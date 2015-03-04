using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Basement.Framework.Configuration
{
    /// <summary>
    /// 自定义数据库config文件读取
    /// </summary>
    public class DatabaseSection : ConfigurationSection
    {
        [ConfigurationProperty("settings")]
        public SettingCollection Settings
        {
            get { return (SettingCollection)this["settings"]; }
        }
    }

    /// <summary>
    /// 包含一个子元素集合的配置元素
    /// </summary>
     [ConfigurationCollection(typeof(SettingElement), AddItemName = "setting", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class SettingCollection : ConfigurationElementCollection
    {
        public SettingElement this[int index]
        {
            get
            {
                return (SettingElement)base.BaseGet(index);
            }
        }

        // 下面二个方法中抽象类中必须要实现的。
        protected override ConfigurationElement CreateNewElement()
        {
            return new SettingElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            var settingElement = element as SettingElement;
            return settingElement != null ? settingElement.Name : "";
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }
        protected override string ElementName
        {
            get { return "setting"; }
        }

    }

    /// <summary>
    /// 配置节点中的配置元素
    /// </summary>   
    public class SettingElement : ConfigurationElement
    {

        [ConfigurationProperty("name", DefaultValue = "", IsRequired = true)]
        public string Name
        {
            get { return this["name"] as string; }
        }

        /// <summary>
        /// 服务器名称
        /// </summary>
        [ConfigurationProperty("server", IsRequired = true)]
        public string Server
        {
            get { return this["server"] as string; }
        }

        /// <summary>
        /// 数据库名
        /// </summary>
        [ConfigurationProperty("dbname", IsRequired = true)]
        public string DbName
        {
            get { return this["dbname"] as string; }
        }
        /// <summary>
        /// 用户名
        /// </summary>
        [ConfigurationProperty("username", IsRequired = true)]
        public string UserName
        {
            get { return this["username"] as string; }
        }
        /// <summary>
        /// 用户密码
        /// </summary>
        [ConfigurationProperty("password", IsRequired = true)]
        public string Password
        {
            get { return this["password"] as string; }
        }

    }

}
