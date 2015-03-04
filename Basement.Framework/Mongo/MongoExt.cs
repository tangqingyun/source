using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Basement.Framework.Mongo
{
    public class MongoExt
    {
        //连接数据库：
        MongoServer server = MongoServer.Create("mongodb://192.168.6.102:30000");
        public MongoServer mongo;
        public MongoServer mongoreadserver;
        public MongoDatabase mongoDatabase;
        public MongoDatabase mongoDatabaseRead;
        public MongoExt(string mongosever, string database, string mongoseverread = "", string databaseread = "")
        {
            
            this.mongo = MongoServer.Create(mongosever);
            // Basement.Framework.Configuration.FrameworkConfig.GetAppSetting();zlrd2012 zhaopin.resume_10386
            //选择DB:
            this.mongoDatabase = server.GetDatabase(database);
            if (!string.IsNullOrWhiteSpace(mongoseverread) && !string.IsNullOrWhiteSpace(databaseread))
            {
                this.mongoreadserver = MongoServer.Create(mongoseverread);
                this.mongoDatabaseRead = server.GetDatabase(databaseread);
            }
        }

        /// <summary>
        /// 插入单个文档
        /// </summary>
        /// <param name="CollectionName">要操作的Mongo集合</param>
        /// <param name="Docment">要插入的单个文档</param>
        /// <param name="IsSafeMode">是否为安全模式插入,true为安全模式(速度慢),false为非安全模式</param>
        /// <example>
        /// 本示例说明如何插入单个文档
        /// <code lang="CS" title="MongoHelper object">
        /// </code>
        /// </example>
        public void InsertSignle(string CollectionName, BsonDocument Docment, bool IsSafeMode)
        {


            using (mongo.RequestStart(mongoDatabase))
            {
                try
                {
                    MongoCollection<BsonDocument> mongoCollection = mongoDatabase.GetCollection<BsonDocument>(CollectionName);
                    mongoCollection.Insert(Docment, IsSafeMode ? SafeMode.True : SafeMode.False);

                }
                catch (Exception ex)
                {

                    throw ex;
                }
                finally
                {
                    //  mongo.Disconnect();
                }

            }


        }

        /// <summary>
        /// 插入多个文档数据(List)
        /// </summary>
        /// <param name="CollectionName">要操作的Mongo集合</param>
        /// <param name="Docments">要插入的多条文档</param>
        /// <param name="IsSafeMode">是否为安全模式插入,true为安全模式,false为非安全模式</param>
        /// <example>
        /// 本示例说明如何插入多个文档数据
        /// <code lang="CS" title="MongoHelper object">
        /// </code>
        /// </example>
        public void InsertMultiple(string CollectionName, IList<BsonDocument> Docments, bool IsSafeMode)
        {

            using (mongo.RequestStart(mongoDatabase))
            {
                try
                {
                    MongoCollection<BsonDocument> mongoCollection = mongoDatabase.GetCollection<BsonDocument>(CollectionName);
                    mongoCollection.InsertBatch(Docments, IsSafeMode ? SafeMode.True : SafeMode.False);

                }
                catch (Exception ex)
                {

                    throw ex;
                }
                finally
                {
                    //mongo.Disconnect();
                }

                //foreach (var docment in Docments)
                //{
                //    mongoCollection.Insert(docment, IsSafeMode ? SafeMode.True : SafeMode.False);
                //}
            }

        }

        public IEnumerable<SafeModeResult> InsertAll(string collectionName, IEnumerable<BsonDocument> entitys)
        {
            IEnumerable<SafeModeResult> result = null;
            if (null == entitys)
            {
                return null;
            }
            using (mongo.RequestStart(mongoDatabase))//开始连接数据库。
            {
                try
                {
                    MongoCollection<BsonDocument> myCollection = mongoDatabase.GetCollection<BsonDocument>(collectionName);
                    result = myCollection.InsertBatch(entitys);

                }
                catch (Exception ex)
                {

                    throw ex;
                }
                finally
                {
                    //  mongo.Disconnect();
                }
            }
            return result;
        }
        /// <summary>
        /// 删除文档
        /// </summary>
        /// <param name="CollectionName">要操作的Mongo集合</param>
        /// <param name="Selector">要删除的文档的条件Document</param>
        /// <param name="IsSafeMode">是否为安全模式插入,true为安全模式,false为非安全模式</param>
        /// <example>
        /// 本示例说明如何删除文档
        /// <code lang="CS" title="MongoHelper object">
        /// </code>
        /// </example>
        public void DeleteDocuments(string CollectionName, BsonDocument Selector, bool IsSafeMode)
        {
            using (mongo.RequestStart(mongoDatabase))
            {
                try
                {
                    // IMongoQuery query = Selector;
                    MongoCollection<BsonDocument> mongoCollection = mongoDatabase.GetCollection<BsonDocument>(CollectionName);
                    mongoCollection.Remove(new QueryDocument(Selector), IsSafeMode ? SafeMode.True : SafeMode.False);

                }
                catch (Exception ex)
                {

                    throw ex;
                }
                finally
                {
                    //  mongo.Disconnect();
                }
                //mongoCollection.Delete(Selector, IsSafeMode); 
            }


        }


        /// <summary>
        /// 查询返回单个文档
        /// </summary>
        /// <param name="CollectionName">要操作的Mongo集合</param>
        /// <param name="QueryDoc">查询条件文档</param>
        /// <returns>返回查询的单个文档</returns>
        /// <example>
        /// 本示例说明如何查询返回单个文档
        /// <code lang="CS" title="MongoHelper object">
        /// </code>
        /// </example>
        public BsonDocument FindSingle(string CollectionName, BsonDocument QueryDoc, bool isReadMster = true)
        {

            return GetOne<BsonDocument>(CollectionName, new QueryDocument(QueryDoc), isReadMster);

        }


        /// <summary>
        /// 查询返回单个文档
        /// </summary>
        /// <param name="CollectionName">要操作的Mongo集合</param>
        /// <param name="QueryDoc">查询条件文档</param>
        /// <returns>返回查询的单个文档</returns>
        /// <example>
        /// 本示例说明如何查询返回单个文档
        /// <code lang="CS" title="MongoHelper object">
        /// </code>
        /// </example>
        public BsonDocument FindSingleReadOnly(string CollectionName, BsonDocument QueryDoc)
        {
            return GetOne<BsonDocument>(CollectionName, new QueryDocument(QueryDoc), false);

        }

        /// <summary>
        /// 查询是否存在条件记录
        /// </summary>
        /// <param name="CollectionName">要操作的Mongo集合</param>
        /// <param name="QueryDoc">查询条件文档</param>
        /// <returns>true为存在,false为不存在</returns>
        /// <example>
        /// 本示例说明查询是否存在条件记录
        /// <code lang="CS" title="MongoHelper object">
        /// </code>
        /// </example>
        public bool Exist(string CollectionName, BsonDocument QueryDoc)
        {

            ObjectId id = GetObjectId(CollectionName, new QueryDocument(QueryDoc));
            if (id == ObjectId.Empty) return false;
            return true;

        }

        /// <summary>
        /// 查询是否存在条件记录
        /// </summary>
        /// <param name="CollectionName">要操作的Mongo集合</param>
        /// <param name="QueryDoc">查询条件文档</param>
        /// <returns>true为存在,false为不存在</returns>
        /// <example>
        /// 本示例说明查询是否存在条件记录
        /// <code lang="CS" title="MongoHelper object">
        /// </code>
        /// </example>
        public bool ExistReadOnly(string CollectionName, BsonDocument QueryDoc)
        {

            ObjectId id = GetObjectId(CollectionName, new QueryDocument(QueryDoc), false);
            if (id == ObjectId.Empty) return false;
            return true;

        }

        public T GetOne<T>(string collectionName, IMongoQuery query, bool isReadMster = true)
        {
            T result = default(T);

            MongoServer mongoc = isReadMster ? mongo : mongoreadserver;
            MongoDatabase mongodbc = isReadMster ? mongoDatabase : mongoDatabaseRead;
            using (mongoc.RequestStart(mongodbc, !isReadMster))
            {

                //ReadPreference preference = ReadPreference.
                try
                {
                    MongoCollection<BsonDocument> myCollection = mongodbc.GetCollection<BsonDocument>(collectionName);
                    if (null == query)
                    {
                        result = myCollection.FindOneAs<T>();
                    }
                    else
                    {
                        result = myCollection.FindOneAs<T>(query);
                    }

                }
                catch (Exception ex)
                {

                    throw ex;
                }
                finally
                {
                    // mongoc.Disconnect();
                }
            }
            return result;
        }

        /// <summary>
        /// 查询某文档id
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="query"></param>
        /// 
        /// <returns></returns>
        private ObjectId GetObjectId(string collectionName, IMongoQuery query, bool isReadMster = true)
        {
            MongoServer mongoc = isReadMster ? mongo : mongoreadserver;
            MongoDatabase mongodbc = isReadMster ? mongoDatabase : mongoDatabaseRead;
            BsonDocument fields = new BsonDocument("_id", 1);
            BsonValue id = BsonNull.Value;
            using (mongoc.RequestStart(mongodbc, !isReadMster))
            {
                try
                {
                    MongoCollection<BsonDocument> mongoCollection = mongodbc.GetCollection<BsonDocument>(collectionName);
                    var cursor = mongoCollection.FindAs<BsonDocument>(query).SetFields(fields.Names.ToArray());

                    if (cursor.Count() > 0)
                    {
                        id = cursor.ToList<BsonDocument>()[0].GetValue("_id");
                    }
                    if (id.IsBsonNull || string.IsNullOrEmpty(id.ToString())) return ObjectId.Empty;

                }

                catch (Exception ex)
                {

                    throw ex;
                }
                finally
                {
                    // mongoc.Disconnect();
                }

                return id.AsObjectId;

            }
            //BsonDocument doc = GetOne<BsonDocument>(collectionName, query);
            //if (null != doc)
            //{
            //    return doc["_id"].AsObjectId;
            //}
            //return ObjectId.Empty;
        }

        /// <summary>
        /// 查询所有数据,不包含返回结果数量以及跳过限制条件
        /// </summary>
        /// <param name="CollectionName">要操作的Mongo集合</param>
        /// <param name="QueryDoc">查询条件文档</param>
        /// <returns>返回查询的结果文档列表</returns>
        /// <example>
        /// 本示例说明如何[查询所有数据,不包含返回结果数量以及跳过限制条件]
        /// <code lang="CS" title="MongoHelper object">
        /// </code>
        /// </example>
        public List<BsonDocument> FindAll(string CollectionName, BsonDocument QueryDoc, bool isReadMster = true)
        {
            MongoServer mongoc = isReadMster ? mongo : mongoreadserver;
            MongoDatabase mongodbc = isReadMster ? mongoDatabase : mongoDatabaseRead;
            using (mongoc.RequestStart(mongodbc, !isReadMster))
            {
                try
                {
                    MongoCollection<BsonDocument> mongoCollection = mongodbc.GetCollection<BsonDocument>(CollectionName);
                    List<BsonDocument> retlist = mongoCollection.FindAs<BsonDocument>(new QueryDocument(QueryDoc)).ToList();

                    return retlist;
                }


                catch (Exception ex)
                {
                    //  return null;
                    throw ex;
                }
                finally
                {
                    // mongoc.Disconnect();
                }

            }


        }


        public long Count(string CollectionName, BsonDocument QueryDoc)
        {
            using (mongo.RequestStart(mongoDatabase))
            {
                try
                {
                    MongoCollection<BsonDocument> mongoCollection = mongoDatabase.GetCollection<BsonDocument>(CollectionName);
                    return mongoCollection.Count(new QueryDocument(QueryDoc));
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    // mongoc.Disconnect();
                }

            }
        }

        public List<BsonDocument> QueryRecords(string CollectionName, BsonDocument QueryDoc, BsonDocument fields, bool isReadMster = true)
        {
            MongoServer mongoc = isReadMster ? mongo : mongoreadserver;
            MongoDatabase mongodbc = isReadMster ? mongoDatabase : mongoDatabaseRead;
            using (mongoc.RequestStart(mongodbc, !isReadMster))
            {
                try
                {
                    MongoCollection<BsonDocument> mongoCollection = mongodbc.GetCollection<BsonDocument>(CollectionName);
                    // List<BsonDocument> retlist = mongoCollection.FindAs<BsonDocument>(new QueryDocument(QueryDoc)).ToList();
                    var cursor = mongoCollection.FindAs<BsonDocument>(new QueryDocument(QueryDoc)).SetFields(fields.Names.ToArray());
                    return cursor.ToList();
                }


                catch (Exception ex)
                {
                    //  return null;
                    throw ex;
                }
                finally
                {
                    // mongoc.Disconnect();
                }

            }


        }

        /// <summary>
        /// 查询所有数据,包含返回结果数量以及跳过限制条件、查询数据总量
        /// </summary>
        /// <param name="CollectionName">要操作的Mongo集合</param>
        /// <param name="QueryDoc">查询条件文档</param>
        /// <returns>返回查询的结果文档列表</returns>
        /// <example>
        /// 本示例说明如何[查询所有数据,不包含返回结果数量以及跳过限制条件]
        /// <code lang="CS" title="MongoHelper object">
        /// </code>
        /// </example>
        public List<BsonDocument> QueryRecordsReadOnly(string CollectionName, BsonDocument QueryDoc)
        {
            using (mongoreadserver.RequestStart(mongoDatabaseRead, true))
            {
                try
                {
                    MongoCollection<BsonDocument> mongoCollection = mongoDatabaseRead.GetCollection<BsonDocument>(CollectionName);

                    var cursor = mongoCollection.FindAs<BsonDocument>(new QueryDocument(QueryDoc)).SetSlaveOk(true);
                    var ret = cursor.ToList<BsonDocument>();

                    return ret;
                }


                catch (Exception ex)
                {
                    //  return null;
                    throw ex;
                }
                finally
                {
                    //  mongoreadserver.Disconnect();
                }

            }


        }

        public long QueryRecordsCount(string CollectionName)
        {

            using (mongoreadserver.RequestStart(mongoDatabaseRead, true))
            {
                try
                {
                    MongoCollection<BsonDocument> mongoCollection = mongoDatabaseRead.GetCollection<BsonDocument>(CollectionName);


                    return mongoCollection.Count();

                }
                catch
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// 查询所有数据,包含返回结果数量以及跳过限制条件、查询数据总量
        /// </summary>
        /// <param name="CollectionName">要操作的Mongo集合</param>
        /// <param name="Skip">查询跳过的记录数</param>
        /// <param name="Limit">返回结果的数量大小</param>
        /// <param name="QueryDoc">查询条件文档</param>
        /// <param name="fields">返回结果的key</param>
        /// <param name="SortBy">排序</param>
        /// <param name="TotalCount">查询结果总量</param>
        /// <returns>返回查询的结果文档列表</returns>
        /// <example>
        /// 本示例说明如何[查询所有数据,不包含返回结果数量以及跳过限制条件]
        /// <code lang="CS" title="MongoHelper object">
        /// </code>
        /// </example>
        public List<BsonDocument> QueryRecordsReadOnly(string CollectionName, int Skip, int Limit, BsonDocument QueryDoc, BsonDocument fields, BsonDocument SortBy, out long TotalCount)
        {


            using (mongoreadserver.RequestStart(mongoDatabaseRead, true))
            {
                try
                {
                    MongoCollection<BsonDocument> mongoCollection = mongoDatabaseRead.GetCollection<BsonDocument>(CollectionName);


                    TotalCount = mongoCollection.FindAs<BsonDocument>(new QueryDocument(QueryDoc)).SetFields(new string[] { "_id" }).ToList().Count();
                    MongoCursor<BsonDocument> cursor;
                    if (fields == null)
                    {
                        cursor = mongoCollection.FindAs<BsonDocument>(new QueryDocument(QueryDoc));
                    }
                    else
                    {
                        cursor = mongoCollection.FindAs<BsonDocument>(new QueryDocument(QueryDoc)).SetFields(fields.Names.ToArray()).SetSlaveOk(true);
                    }
                    cursor.SetSkip(Skip);
                    cursor.SetLimit(Limit);
                    SortByDocument sby = new SortByDocument();
                    sby.Add(SortBy);
                    cursor.SetSortOrder(sby);
                    var ret = cursor.ToList<BsonDocument>();

                    return ret;
                }


                catch (Exception ex)
                {
                    //  return null;
                    throw ex;
                }
                finally
                {
                    // mongoreadserver.Disconnect();
                }
            }


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CollectionName"></param>
        /// <param name="QueryDoc"></param>
        /// <param name="fields"></param>
        /// <param name="totalCount"></param>
        /// <param name="t1"></param>
        /// <returns></returns>
        public List<BsonDocument> QueryRecordsReadOnly(string CollectionName, BsonDocument QueryDoc, BsonDocument fields, out long totalCount, out long t1)
        {
            using (mongoreadserver.RequestStart(mongoDatabaseRead, true))
            {
                try
                {
                    System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                    sw.Start();
                    MongoCollection<BsonDocument> mongoCollection = mongoDatabaseRead.GetCollection<BsonDocument>(CollectionName);
                    var cursor = mongoCollection.FindAs<BsonDocument>(new QueryDocument(QueryDoc)).SetFields(fields.Names.ToArray()).SetSlaveOk(true);
                    totalCount = cursor.Count();
                    sw.Stop();
                    t1 = sw.ElapsedMilliseconds;

                    return cursor.ToList<BsonDocument>();
                }

                catch (Exception ex)
                {
                    //  return null;
                    throw ex;
                }
                finally
                {
                    //   mongoreadserver.Disconnect();
                }
            }


        }

        
        /// <summary>
        /// 
        /// </summary>
        public class Mongo2Cols
        {
            public long campusID { get; set; }
            public string orderCol { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CollectionName"></param>
        /// <param name="QueryDoc"></param>
        /// <param name="fields"></param>
        /// <param name="orderColName"></param>
        /// <param name="totalCount"></param>
        /// <param name="t1"></param>
        /// <returns></returns>
        public List<Mongo2Cols> QueryRecords4CampusOrder(string CollectionName, BsonDocument QueryDoc, BsonDocument fields, string orderColName, out long totalCount, out long t1, bool isReadMster = false)
        {
            //这里应该默认读从库
            List<Mongo2Cols> docs = new List<Mongo2Cols>();
            MongoServer mongoc = isReadMster ? mongo : mongoreadserver;
            MongoDatabase mongodbc = isReadMster ? mongoDatabase : mongoDatabaseRead;
            using (mongoc.RequestStart(mongodbc, !isReadMster))
            {
                try
                {
                    System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();


                    MongoCollection<BsonDocument> mongoCollection = mongodbc.GetCollection<BsonDocument>(CollectionName);
                    var cursor = mongoCollection.FindAs<BsonDocument>(new QueryDocument(QueryDoc)).SetFields(fields.Names.ToArray());
                    sw.Start();
                    foreach (var doc in cursor)
                    {
                        Mongo2Cols cols = new Mongo2Cols();
                        long campusid = 0;
                        long.TryParse(doc["campus_id"].ToString(), out campusid);
                        cols.campusID = campusid;

                        string[] orderCols = orderColName.Split('.');

                        BsonDocument doc1 = null;

                        int length = orderCols.Length;

                        for (int i = 0; i < length; i++)
                        {
                            if (length == 1)
                            {
                                cols.orderCol = doc[orderColName] != null ? doc[orderColName].ToString() : "";
                            }
                            else
                            {
                                if (i == 0)
                                {
                                    doc1 = (BsonDocument)doc[orderCols[i]];
                                }
                                else
                                {
                                    if (i != length - 1)
                                    {
                                        doc1 = (BsonDocument)doc1[orderCols[i]];
                                    }
                                    else
                                    {
                                        cols.orderCol = doc1[orderCols[length - 1]] != null ? doc1[orderCols[length - 1]].ToString() : "";
                                    }
                                }
                            }
                        }

                        docs.Add(cols);
                    }

                    totalCount = docs.Count;
                    sw.Stop();
                    t1 = sw.ElapsedMilliseconds;

                    return docs;
                }
                catch (Exception ex)
                {
                    //  return null;
                    throw ex;
                }
                finally
                {
                    //  mongoc.Disconnect();
                }
            }


        }


        /// <summary>
        /// 查询所有数据,包含返回结果数量以及跳过限制条件、查询数据总量
        /// </summary>
        /// <param name="CollectionName">要操作的Mongo集合</param>
        /// <param name="Skip">查询跳过的记录数</param>
        /// <param name="Limit">返回结果的数量大小</param>
        /// <param name="QueryDoc">查询条件文档</param>
        /// <param name="fields">返回结果的key</param>
        /// <param name="SortBy">排序</param>
        /// <param name="TotalCount">查询结果总量</param>
        /// <returns>返回查询的结果文档列表</returns>
        /// <example>
        /// 本示例说明如何[查询所有数据,不包含返回结果数量以及跳过限制条件]
        /// </example>
        public List<BsonDocument> QueryRecords(string CollectionName, int Skip, int Limit, BsonDocument QueryDoc, BsonDocument fields, BsonDocument SortBy, out long TotalCount, bool isReadMster = true)
        {

            MongoServer mongoc = isReadMster ? mongo : mongoreadserver;
            MongoDatabase mongodbc = isReadMster ? mongoDatabase : mongoDatabaseRead;
            using (mongoc.RequestStart(mongodbc, !isReadMster))
            {
                try
                {
                    MongoCollection<BsonDocument> mongoCollection = mongodbc.GetCollection<BsonDocument>(CollectionName);
                    TotalCount = mongoCollection.Find(new QueryDocument(QueryDoc)).SetFields(new string[] { "_id" }).ToList().Count();
                    var cursor = mongoCollection.Find(new QueryDocument(QueryDoc));

                    cursor.SetSkip(Skip);
                    cursor.SetLimit(Limit);
                    cursor.SetFields(fields.Names.ToArray());
                    SortByDocument sby = new SortByDocument();
                    sby.Add(SortBy);
                    cursor.SetSortOrder(sby);
                    var ret = cursor.ToList();

                    return ret;
                }
                catch (Exception ex)
                {
                    //  return null;
                    throw ex;
                }
                finally
                {
                    // mongoc.Disconnect();
                }
            }


        }

        public List<BsonDocument> QueryRecords(string CollectionName, IMongoQuery query, string indexName, string lastIndexValue, int Limit, bool isReadMster = true)
        {
            MongoServer mongoc = isReadMster ? mongo : mongoreadserver;
            MongoDatabase mongodbc = isReadMster ? mongoDatabase : mongoDatabaseRead;
            if (query == null)
            {
                //类似SQL：1=1
                query = Query.Exists("_id");
            }
#pragma warning disable 612,618
            using (mongoc.RequestStart(mongodbc, !isReadMster))
#pragma warning restore 612,618
            {
                //默认升序
                MongoCursor<BsonDocument> mongoCursor = null;
                try
                {
                    MongoCollection<BsonDocument> mc = mongodbc.GetCollection<BsonDocument>(CollectionName);
                    if (lastIndexValue != null)
                    {

                        //有上一批最大主键的值传进来时才添加条件
                        query = Query.And(query, Query.GT(indexName, BsonValue.Create(new ObjectId(lastIndexValue))));
                    }
                    //先按条件查询 再排序 再取数
                    mongoCursor = mc.Find(query).SetSortOrder(new SortByDocument(indexName, 1)).SetLimit(Limit);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return mongoCursor.ToList();
            }
        }


        /// <summary>
        /// 更新文档数据
        /// </summary>
        /// <param name="CollectionName">要操作的Mongo集合</param>
        /// <param name="OldDocment">用于搜索到需要更新的文档查询条件文档</param>
        /// <param name="NewDocment">更新内容</param>
        /// <param name="IsSafeMode">是否为安全模式插入,true为安全模式,false为非安全模式</param>
        public void Update(string CollectionName, BsonDocument OldDocment, BsonDocument NewDocment, bool IsSafeMode)
        {
            using (mongo.RequestStart(mongoDatabase))
            {
                try
                {
                    BsonDocument updatedoc = new BsonDocument { { "$set", NewDocment } };
                    MongoCollection<BsonDocument> mongoCollection = mongoDatabase.GetCollection<BsonDocument>(CollectionName);
                    mongoCollection.Update(new QueryDocument(OldDocment), new UpdateDocument(updatedoc), IsSafeMode ? SafeMode.True : SafeMode.False);
                    mongo.Disconnect();
                }
                catch (Exception ex)
                {
                    //  return null;
                    throw ex;
                }
                finally
                {
                    //  mongo.Disconnect();
                }
            }

        }

        /// <summary>
        /// 更新文档数据
        /// </summary>
        /// <param name="CollectionName">要操作的Mongo集合</param>
        /// <param name="OldDocment">用于搜索到需要更新的文档查询条件文档</param>
        /// <param name="NewDocment">更新内容</param>
        /// <param name="IsSafeMode">是否为安全模式插入,true为安全模式,false为非安全模式</param>
        /// <param name="UpdateFlag">更新模式:1(MultiUpdate),2(None),3(Upsert)</param>
        public void Update(string CollectionName, BsonDocument OldDocment, BsonDocument NewDocment, bool IsSafeMode, int UpdateFlag)
        {

            using (mongo.RequestStart(mongoDatabase))
            {
                // IMongoUpdate up = new UpdateDocument();
                try
                {
                    MongoCollection<BsonDocument> mongoCollection = mongoDatabase.GetCollection<BsonDocument>(CollectionName);
                    BsonDocument updatedoc = new BsonDocument { { "$set", NewDocment } };
                    switch (UpdateFlag)
                    {
                        //UpdateFlags.Upsert: 如果不存在Update的记录，插objNew
                        //UpdateFlags.MultiUpdate: 把按条件查出来多条记录全部更新. 
                        case 1:
                            mongoCollection.Update(new QueryDocument(OldDocment), new UpdateDocument(updatedoc), UpdateFlags.Multi, IsSafeMode ? SafeMode.True : SafeMode.False);
                            break;
                        case 2:
                            mongoCollection.Update(new QueryDocument(OldDocment), new UpdateDocument(updatedoc), UpdateFlags.None, IsSafeMode ? SafeMode.True : SafeMode.False);

                            break;
                        case 3:
                            mongoCollection.Update(new QueryDocument(OldDocment), new UpdateDocument(updatedoc), UpdateFlags.Upsert, IsSafeMode ? SafeMode.True : SafeMode.False);

                            break;
                    }

                }
                catch (Exception ex)
                {
                    //  return null;
                    throw ex;
                }
                finally
                {
                    //  mongo.Disconnect();
                }
            }

        }

        /// <summary>
        /// 更新文档数据
        /// </summary>
        /// <param name="CollectionName">要操作的Mongo集合</param>
        /// <param name="OldDocment">用于搜索到需要更新的文档查询条件文档</param>
        /// <param name="NewDocment">更新内容</param>
        /// <example>
        /// 本示例说明查询是否存在条件记录
        /// <code lang="CS" title="MongoHelper object">
        /// string connString = ConnectionStringProvider.GetSqlServerCONString("Mongo_ConnString");
        /// string dBname = ConnectionStringProvider.GetSqlServerCONString("Mongo_DBName");
        /// MongoDBHelper defaultHelper = new MongoDBHelper(connString,dBname);
        /// string TableName = "zhaopin.resume";
        /// Document OldDoc = new Document();
        /// OldDoc.Add("name","tom");
        /// OldDoc.Add("age",25);
        /// Document NewDoc = new Document();
        /// NewDoc.Add("name","tom");
        /// NewDoc.Add("age",26);
        /// defaultHelper.FindAndModify(TableName, OldDoc, NewDoc);
        /// </code>
        /// </example>
        public void FindAndModify(string CollectionName, BsonDocument OldDocment, BsonDocument NewDocment)
        {

            using (mongo.RequestStart(mongoDatabase))
            {
                try
                {
                    BsonDocument updatedoc = new BsonDocument { { "$set", NewDocment } };
                    MongoCollection<BsonDocument> mongoCollection = mongoDatabase.GetCollection<BsonDocument>(CollectionName);
                    mongoCollection.Update(new QueryDocument(OldDocment), new UpdateDocument(updatedoc), UpdateFlags.Multi, SafeMode.False);
                    //  mongoCollection.FindAndModify(new QueryDocument(OldDocment), new SortByDocument("root_company_id", 1), new UpdateDocument(NewDocment), true);


                }
                catch (Exception ex)
                {
                    //  return null;
                    throw ex;
                }
                finally
                {
                    // mongo.Disconnect();
                }
                // mongoCollection.FindAndModify(NewDocment, OldDocment);
            }

        }

        /// <summary>
        /// 创建索引 这里一定要链接主库
        /// </summary>
        /// <param name="collectionName">集合名称</param>
        /// <param name="indexKeys">索引键数组</param>
        public void EnsureIndex(string collectionName, IEnumerable<string> indexKeys)
        {
            CommandShard(collectionName);
            using (mongo.RequestStart(mongoDatabase))
            {
                MongoCollection<BsonDocument> mongoCollection = mongoDatabase.GetCollection<BsonDocument>(collectionName);

                foreach (var indexKey in indexKeys)
                {
                    try
                    {
                        if (indexKey.IndexOf(',') > 0)
                        {
                            var keys = indexKey.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                            mongoCollection.EnsureIndex(keys);
                        }
                        else
                        {
                            mongoCollection.EnsureIndex(indexKey);
                        }
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }

                }
                //  mongo.Disconnect();
            }
        }
        /// <summary>
        /// 添加表分片
        /// </summary>
        /// <param name="collectionName"></param>
        private void CommandShard(string collectionName)
        {
            MongoDatabase mongoDatabase1 = mongo.GetDatabase("admin");
            string shardName = mongoDatabase.Name + "." + collectionName;
            // using (mongo.RequestStart(mongoDatabase1))
            {
                BsonDocument fields = new BsonDocument("shardcollection", shardName);
                fields.Add("key", new BsonDocument("campus_id", "hashed"));
                IMongoCommand mc = new CommandDocument(fields);
                mongoDatabase1.RunCommand(mc);
            }
        }


    }

}
