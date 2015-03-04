using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Reflection.Emit;


namespace Basement.Framework.Data.Core
{
    public static class DataReaderExtensions
    {
        private static readonly MethodInfo DataRecord_ItemGetter_Int = typeof(IDataRecord).GetMethod("get_Item", new Type[] { typeof(int) });
        private static readonly MethodInfo DataRecord_GetOrdinal = typeof(IDataRecord).GetMethod("GetOrdinal");
        private static readonly MethodInfo DataReader_Read = typeof(DbDataReader).GetMethod("Read");
        private static readonly MethodInfo Convert_IsDBNull = typeof(Convert).GetMethod("IsDBNull");
        private static readonly MethodInfo DataRecord_GetDateTime = typeof(IDataRecord).GetMethod("GetDateTime");
        private static readonly MethodInfo DataRecord_GetDecimal = typeof(IDataRecord).GetMethod("GetDecimal");
        private static readonly MethodInfo DataRecord_GetDouble = typeof(IDataRecord).GetMethod("GetDouble");
        private static readonly MethodInfo DataRecord_GetInt32 = typeof(IDataRecord).GetMethod("GetInt32");
        private static readonly MethodInfo DataRecord_GetInt64 = typeof(IDataRecord).GetMethod("GetInt64");
        private static readonly MethodInfo DataRecord_IsDBNull = typeof(IDataRecord).GetMethod("IsDBNull");

        public static IEnumerable<T> ToList<T>(this IDataReader reader)
        {
            if (reader == null) throw new ArgumentNullException("Data.Core [DataReaderExtensions] : Reader is null.");
                return EntityConverter<T>.ToList(reader);
        }

        public class EntityConverter<T>
        {
            private static Converter<IDataReader, IEnumerable<T>> batchDataLoader;
            private static Converter<IDataReader, IEnumerable<T>> BatchDataLoader
            {
                get
                {
                    if (batchDataLoader == null) batchDataLoader = CreateBatchDataLoader(Entity<T>.EntityProperties);
                    return batchDataLoader;
                }
            }

            private static Converter<IDataReader, IEnumerable<T>> CreateBatchDataLoader(IList<EntityProperty> columnInfoes)
            {
                    DynamicMethod dm = new DynamicMethod(string.Empty, typeof(IEnumerable<T>), new Type[] { typeof(IDataReader) }, typeof(EntityConverter<T>));
                    ILGenerator il = dm.GetILGenerator();
                    LocalBuilder list = il.DeclareLocal(typeof(IEnumerable<T>));
                    LocalBuilder item = il.DeclareLocal(typeof(T));
                    Label exit = il.DefineLabel();
                    Label loop = il.DefineLabel();
                    il.Emit(OpCodes.Newobj, typeof(List<T>).GetConstructor(Type.EmptyTypes));
                    il.Emit(OpCodes.Stloc_S, list);
                    LocalBuilder[] colIndices = GetColumnIndices(il, columnInfoes);
                    il.MarkLabel(loop);
                    il.Emit(OpCodes.Ldarg_0);
                    il.Emit(OpCodes.Callvirt, DataReader_Read);
                    il.Emit(OpCodes.Brfalse, exit);
                    BuildItem(il, columnInfoes, item, colIndices);
                    il.Emit(OpCodes.Ldloc_S, list);
                    il.Emit(OpCodes.Ldloc_S, item);
                    il.Emit(OpCodes.Callvirt, typeof(List<T>).GetMethod("Add"));
                    il.Emit(OpCodes.Br, loop);
                    il.MarkLabel(exit);
                    il.Emit(OpCodes.Ldloc_S, list);
                    il.Emit(OpCodes.Ret);
                    return (Converter<IDataReader, IEnumerable<T>>)dm.CreateDelegate(typeof(Converter<IDataReader, IEnumerable<T>>));
            }

            private static LocalBuilder[] GetColumnIndices(ILGenerator il, IList<EntityProperty> columnInfoes)
            {
                LocalBuilder[] colIndices = new LocalBuilder[columnInfoes.Count];
                    for (int i = 0; i < colIndices.Length; i++)
                    {
                        colIndices[i] = il.DeclareLocal(typeof(int));
                        il.Emit(OpCodes.Ldarg_0);
                        il.Emit(OpCodes.Ldstr, columnInfoes[i].ColumnName);
                        il.Emit(OpCodes.Callvirt, DataRecord_GetOrdinal);
                        il.Emit(OpCodes.Stloc_S, colIndices[i]);
                    }
                return colIndices;
            }

            private static void BuildItem(ILGenerator il, IList<EntityProperty> columnInfoes, LocalBuilder item, LocalBuilder[] colIndices)
            {
                // T item = new T();
                il.Emit(OpCodes.Newobj, Entity<T>.EntityType.GetConstructor(Type.EmptyTypes));
                il.Emit(OpCodes.Stloc_S, item);

                for (int i = 0; i < colIndices.Length; i++)
                {
                    //il.BeginExceptionBlock();
                    ////////////////////////////////////
                    if (IsCompatibleType(columnInfoes[i].PropertyType, typeof(int)))
                    {
                        ReadInt32(il, item, columnInfoes, colIndices, i);
                    }
                    else if (IsCompatibleType(columnInfoes[i].PropertyType, typeof(int?)))
                    {
                        ReadNullableInt32(il, item, columnInfoes, colIndices, i);
                    }
                    else if (IsCompatibleType(columnInfoes[i].PropertyType, typeof(long)))
                    {
                        ReadInt64(il, item, columnInfoes, colIndices, i);
                    }
                    else if (IsCompatibleType(columnInfoes[i].PropertyType, typeof(long?)))
                    {
                        ReadNullableInt64(il, item, columnInfoes, colIndices, i);
                    }
                    else if (IsCompatibleType(columnInfoes[i].PropertyType, typeof(decimal)))
                    {
                        ReadDecimal(il, item, columnInfoes[i].SetMethod, colIndices[i]);
                    }
                    else if (columnInfoes[i].PropertyType == typeof(decimal?))
                    {
                        ReadNullableDecimal(il, item, columnInfoes[i].SetMethod, colIndices[i]);
                    }
                    else if (columnInfoes[i].PropertyType == typeof(DateTime))
                    {
                        ReadDateTime(il, item, columnInfoes[i].SetMethod, colIndices[i]);
                    }
                    else if (columnInfoes[i].PropertyType == typeof(DateTime?))
                    {
                        ReadNullableDateTime(il, item, columnInfoes[i].SetMethod, colIndices[i]);
                    }
                    else
                    {
                        ReadObject(il, item, columnInfoes, colIndices, i);
                    }
                    ////////////////////////////////////
                    //il.BeginCatchBlock(typeof(InvalidCastException));
                    //il.Emit(OpCodes.Ldstr, string.Format("Type conversion failure. {0}-{1}", Entity<T>.EntityType.Name, columnInfoes[i].PropertyName));
                    //il.Emit(OpCodes.Newobj, typeof(InvalidCastException).GetConstructor(new Type[] { typeof(string) }));
                    //il.Emit(OpCodes.Throw);
                    ////il.Emit(OpCodes.Call, typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }));
                    //// il.Emit(OpCodes.Pop);
                    //il.EndExceptionBlock();
                    ////il.Emit(OpCodes.Ret);
                }
            }

            private static bool IsCompatibleType(Type t1, Type t2)
            {
                
                if (t1 == t2)
                    return true;
                if (t1.IsEnum && System.Enum.GetUnderlyingType(t1) == t2)
                    return true;
                var u1 = Nullable.GetUnderlyingType(t1);
                var u2 = Nullable.GetUnderlyingType(t2);
                if (u1 != null && u2 != null)
                    return IsCompatibleType(u1, u2);
                return false;
            }

            private static void ReadInt32(ILGenerator il, LocalBuilder item, IList<EntityProperty> columnInfoes, LocalBuilder[] colIndices, int i)
            {
                il.Emit(OpCodes.Ldloc_S, item);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldloc_S, colIndices[i]);
                il.Emit(OpCodes.Callvirt, DataRecord_GetInt32);
                il.Emit(OpCodes.Callvirt, columnInfoes[i].SetMethod);
            }

            private static void ReadNullableInt32(ILGenerator il, LocalBuilder item, IList<EntityProperty> columnInfoes, LocalBuilder[] colIndices, int i)
            {
                var local = il.DeclareLocal(columnInfoes[i].PropertyType);
                Label intNull = il.DefineLabel();
                Label intCommon = il.DefineLabel();
                il.Emit(OpCodes.Ldloca, local);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldloc_S, colIndices[i]);
                il.Emit(OpCodes.Callvirt, DataRecord_IsDBNull);
                il.Emit(OpCodes.Brtrue_S, intNull);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldloc_S, colIndices[i]);
                il.Emit(OpCodes.Callvirt, DataRecord_GetInt32);
                il.Emit(OpCodes.Call, columnInfoes[i].PropertyType.GetConstructor(new Type[] { Nullable.GetUnderlyingType(columnInfoes[i].PropertyType) }));
                il.Emit(OpCodes.Br_S, intCommon);
                il.MarkLabel(intNull);
                il.Emit(OpCodes.Initobj, columnInfoes[i].PropertyType);
                il.MarkLabel(intCommon);
                il.Emit(OpCodes.Ldloc_S, item);
                il.Emit(OpCodes.Ldloc, local);
                il.Emit(OpCodes.Callvirt, columnInfoes[i].SetMethod);
            }

            private static void ReadInt64(ILGenerator il, LocalBuilder item, IList<EntityProperty> columnInfoes, LocalBuilder[] colIndices, int i)
            {
                il.Emit(OpCodes.Ldloc_S, item);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldloc_S, colIndices[i]);
                il.Emit(OpCodes.Callvirt, DataRecord_GetInt64);
                il.Emit(OpCodes.Callvirt, columnInfoes[i].SetMethod);
            }

            private static void ReadNullableInt64(ILGenerator il, LocalBuilder item, IList<EntityProperty> columnInfoes, LocalBuilder[] colIndices, int i)
            {
                var local = il.DeclareLocal(columnInfoes[i].PropertyType);
                Label intNull = il.DefineLabel();
                Label intCommon = il.DefineLabel();
                il.Emit(OpCodes.Ldloca, local);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldloc_S, colIndices[i]);
                il.Emit(OpCodes.Callvirt, DataRecord_IsDBNull);
                il.Emit(OpCodes.Brtrue_S, intNull);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldloc_S, colIndices[i]);
                il.Emit(OpCodes.Callvirt, DataRecord_GetInt64);
                il.Emit(OpCodes.Call, columnInfoes[i].PropertyType.GetConstructor(new Type[] { Nullable.GetUnderlyingType(columnInfoes[i].PropertyType) }));
                il.Emit(OpCodes.Br_S, intCommon);
                il.MarkLabel(intNull);
                il.Emit(OpCodes.Initobj, columnInfoes[i].PropertyType);
                il.MarkLabel(intCommon);
                il.Emit(OpCodes.Ldloc_S, item);
                il.Emit(OpCodes.Ldloc, local);
                il.Emit(OpCodes.Callvirt, columnInfoes[i].SetMethod);
            }

            private static void ReadDecimal(ILGenerator il, LocalBuilder item, MethodInfo setMethod, LocalBuilder colIndex)
            {
                il.Emit(OpCodes.Ldloc_S, item);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldloc_S, colIndex);
                il.Emit(OpCodes.Callvirt, DataRecord_GetDecimal);
                il.Emit(OpCodes.Callvirt, setMethod);
            }

            private static void ReadNullableDecimal(ILGenerator il, LocalBuilder item, MethodInfo setMethod, LocalBuilder colIndex)
            {
                var local = il.DeclareLocal(typeof(decimal?));
                Label decimalNull = il.DefineLabel();
                Label decimalCommon = il.DefineLabel();
                il.Emit(OpCodes.Ldloca, local);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldloc_S, colIndex);
                il.Emit(OpCodes.Callvirt, DataRecord_IsDBNull);
                il.Emit(OpCodes.Brtrue_S, decimalNull);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldloc_S, colIndex);
                il.Emit(OpCodes.Callvirt, DataRecord_GetDecimal);
                il.Emit(OpCodes.Call, typeof(decimal?).GetConstructor(new Type[] { typeof(decimal) }));
                il.Emit(OpCodes.Br_S, decimalCommon);
                il.MarkLabel(decimalNull);
                il.Emit(OpCodes.Initobj, typeof(decimal?));
                il.MarkLabel(decimalCommon);
                il.Emit(OpCodes.Ldloc_S, item);
                il.Emit(OpCodes.Ldloc, local);
                il.Emit(OpCodes.Callvirt, setMethod);
            }

            private static void ReadDateTime(ILGenerator il, LocalBuilder item, MethodInfo setMethod, LocalBuilder colIndex)
            {
                il.Emit(OpCodes.Ldloc_S, item);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldloc_S, colIndex);
                il.Emit(OpCodes.Callvirt, DataRecord_GetDateTime);
                il.Emit(OpCodes.Callvirt, setMethod);
            }

            private static void ReadNullableDateTime(ILGenerator il, LocalBuilder item, MethodInfo setMethod, LocalBuilder colIndex)
            {
                var local = il.DeclareLocal(typeof(DateTime?));
                Label dtNull = il.DefineLabel();
                Label dtCommon = il.DefineLabel();
                il.Emit(OpCodes.Ldloca, local);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldloc_S, colIndex);
                il.Emit(OpCodes.Callvirt, DataRecord_IsDBNull);
                il.Emit(OpCodes.Brtrue_S, dtNull);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldloc_S, colIndex);
                il.Emit(OpCodes.Callvirt, DataRecord_GetDateTime);
                il.Emit(OpCodes.Call, typeof(DateTime?).GetConstructor(new Type[] { typeof(DateTime) }));
                il.Emit(OpCodes.Br_S, dtCommon);
                il.MarkLabel(dtNull);
                il.Emit(OpCodes.Initobj, typeof(DateTime?));
                il.MarkLabel(dtCommon);
                il.Emit(OpCodes.Ldloc_S, item);
                il.Emit(OpCodes.Ldloc, local);
                il.Emit(OpCodes.Callvirt, setMethod);
            }

            private static void ReadObject(ILGenerator il, LocalBuilder item, IList<EntityProperty> columnInfoes, LocalBuilder[] colIndices, int i)
            {
                Label common = il.DefineLabel();
                il.Emit(OpCodes.Ldloc_S, item);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldloc_S, colIndices[i]);
                il.Emit(OpCodes.Callvirt, DataRecord_ItemGetter_Int);
                il.Emit(OpCodes.Dup);
                il.Emit(OpCodes.Call, Convert_IsDBNull);
                il.Emit(OpCodes.Brfalse_S, common);
                il.Emit(OpCodes.Pop);
                il.Emit(OpCodes.Ldnull);
                il.MarkLabel(common);
                il.Emit(OpCodes.Unbox_Any, columnInfoes[i].PropertyType);
                il.Emit(OpCodes.Callvirt, columnInfoes[i].SetMethod);
            }

            public static IEnumerable<T> ToList(IDataReader reader)
            {
                return BatchDataLoader(reader);
            }
        }
    }
}
