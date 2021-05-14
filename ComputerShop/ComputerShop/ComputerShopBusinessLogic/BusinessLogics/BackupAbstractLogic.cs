using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Runtime.Serialization.Json;
using System.Collections;
using System.Linq;

namespace ComputerShopBusinessLogic.BusinessLogics
{
    public abstract class BackupAbstractLogic
    {
        public void CreateArchive(string folderName)
        {
            try
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(folderName);
                if(directoryInfo.Exists)
                {
                    foreach (FileInfo file in directoryInfo.GetFiles())
                    {
                        file.Delete();
                    }
                }

                string fileName = $"{folderName}.zip";
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }

                var assembly = GetAssembly();
                var dbsets = GetFullList();
                var methodInfo = GetType().BaseType.GetTypeInfo().GetDeclaredMethod("SaveToFile");

                foreach (var set in dbsets)
                {
                    var element = assembly.CreateInstance(set.PropertyType.GenericTypeArguments[0].FullName);
                    var genericMethodInfo = methodInfo.MakeGenericMethod(element.GetType());
                    genericMethodInfo.Invoke(this, new object[] { folderName });
                }

                ZipFile.CreateFromDirectory(folderName,fileName);
                directoryInfo.Delete(true);
            }
            catch (Exception) { throw; }
        }

        private void SaveToFile<T>(string folderName) where T : class, new()
        {
            var records = GetList<T>();
            T obj = new T();

            if(records != null)
            {
                var xDocument = new XDocument(GetXElement(records));
                xDocument.Save(string.Format("{0}/{1}.xml", folderName, obj.GetType().Name));
            }
        }

        private XElement GetXElement(IEnumerable list) 
        {
            var currentType = list.GetType().GetGenericArguments()[0];
            var xElement = new XElement(currentType.Name+"s");
            foreach (var item in list)
            {
                var xElementItem = new XElement(item.GetType().Name);
                foreach (var propertyInfo in currentType.GetProperties())
                {
                    if (propertyInfo.GetValue(item) != null)
                    {
                        if (propertyInfo.PropertyType != typeof(string) && typeof(ICollection).IsAssignableFrom(propertyInfo.PropertyType) && typeof(IEnumerable).IsAssignableFrom(propertyInfo.PropertyType))
                        {
                            xElementItem.Add(GetXElement(propertyInfo.GetValue(item) as IEnumerable));
                        }
                        else
                        {
                            xElementItem.Add(new XElement(propertyInfo.Name, propertyInfo.GetValue(item)));
                        }
                    }
                }
                xElement.Add(xElementItem);
            }
            return xElement;
        }

        protected abstract Assembly GetAssembly();
        protected abstract List<PropertyInfo> GetFullList();
        protected abstract List<T> GetList<T>() where T : class, new();
    }
}
