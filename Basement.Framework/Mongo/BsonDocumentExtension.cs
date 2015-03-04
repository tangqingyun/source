using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;

namespace Basement.Framework.Mongo
{
    public static class BsonDocumentExtension
    {

        public static bool ContainsBsonKey(this BsonDocument data, string Key)
        {
            if (data == null || data == BsonNull.Value || string.IsNullOrEmpty(Key)) return false;
            try
            {
                if (Key.Contains("."))
                {
                    string[] keyopts = Key.Split(new Char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

                    BsonDocument _retvalue = data;
                    for (int i = 0; i < keyopts.Length; i++)
                    {
                        if (_retvalue.Contains(keyopts[i]))
                        {

                            if (_retvalue[keyopts[i]].IsBsonDocument && !i.Equals(keyopts.Length - 1))
                            {
                                _retvalue = _retvalue[keyopts[i]] as BsonDocument;
                            }
                            else
                            {
                                //如果不是bsondocument 是bsonvalue那么i应该是最后一个key
                                return i.Equals(keyopts.Length - 1);

                            }

                        }
                        else
                        {
                            return false;
                        }

                    }
                    return false;
                }
                else
                {
                    return data.Contains(Key);
                }
            }
            catch
            {
                return false;
            }
        }
        public static BsonValue GetBsonValue(this BsonDocument data, string Key)
        {

            BsonValue _retvalue = null;
            if (data == null || data == BsonNull.Value || string.IsNullOrEmpty(Key)) return BsonNull.Value;
            try
            {
                if (Key.Contains("."))
                {
                    string[] keyopts = Key.Split(new Char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                    _retvalue = (BsonValue)data[keyopts[0]];
                    for (int i = 1; i < keyopts.Length; i++)
                    {
                        BsonDocument doc = _retvalue as BsonDocument;
                        _retvalue = (BsonValue)doc[keyopts[i]];
                        if (!_retvalue.IsBsonDocument)
                        {

                            break;
                        }
                    }
                }
                else
                {
                    _retvalue = (BsonValue)data[Key];

                }
            }
            catch
            { _retvalue = BsonNull.Value; }
            return _retvalue;
        }


        public static BsonElement GetBsonElement(this BsonDocument doc, string Key)
        {
            BsonElement _retvalue = null;
            if (doc == null || doc == BsonNull.Value || string.IsNullOrEmpty(Key)) return null;
            try
            {
                if (Key.Contains("."))
                {
                    string[] keyopts = Key.Split(new Char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                    _retvalue = doc.GetElement(keyopts[0]);
                    for (int i = 1; i < keyopts.Length; i++)
                    {
                        BsonDocument sonDoc = _retvalue.Value as BsonDocument;
                        _retvalue = sonDoc.GetElement(keyopts[i]);
                        if (!_retvalue.Value.IsBsonDocument)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    _retvalue = doc.GetElement(Key);

                }
            }
            catch
            { _retvalue = null; }
            return _retvalue;
        }
        /// <summary>
        /// 只适用于循环模块
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static BsonDocument GetBsonDocumentByS(this BsonDocument data)
        {

            string moudlename = string.Empty;
            string parentname = string.Empty;
            BsonDocument mdoc = new BsonDocument();
            BsonDocument doc = new BsonDocument();
            int i = 1;
            int count = data.Elements.Count();
            foreach (var key in data.Elements)
            {
                string[] values = key.Name.Split(new Char[] { '.' });
                if (!string.IsNullOrEmpty(parentname) && i == count)
                {
                    moudlename = values[0];
                    parentname = values[1];
                    doc.Add(values[2], key.Value);
                    //  BsonDocument childdoc =;
                    mdoc.Add(new BsonDocument(parentname, doc.Clone()));
                    // doc = new BsonDocument();
                    break;

                }
                else if (!string.IsNullOrEmpty(parentname) && !parentname.Equals(values[1]))
                {
                    //  BsonDocument childdoc =;
                    mdoc.Add(new BsonDocument(parentname, doc.Clone()));
                    doc = new BsonDocument();
                    moudlename = values[0];
                    parentname = values[1];
                    doc.Add(values[2], key.Value);

                }

                else
                {
                    moudlename = values[0];
                    parentname = values[1];
                    doc.Add(values[2], key.Value);

                }



                i++;

            }

            return new BsonDocument(moudlename, mdoc);

        }

    }
}
