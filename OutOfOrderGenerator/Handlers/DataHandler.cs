using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using OutOfOrderGenerator.ViewModels;
using System.IO;

namespace OutOfOrderGenerator.Handlers
{
    public class DataHandler
    {
        public string dataPath = System.Environment.CurrentDirectory + @"\DataSave.json";
        public DataHandler()
        {
            if (!File.Exists(dataPath))
            {
                var myfile = File.Create(dataPath);
                myfile.Close();
                //文件创建时写入默认的公司名
                GongsiNameList gn = new GongsiNameList();
                gn.Add(new GongsiName() { Name = "山西杏花村汾酒厂股份有限公司" });
                WriteData(gn);
            }
        }

        public GongsiNameList ReadData()
        {
            GongsiNameList gongsis;
            using(StreamReader sr = new StreamReader(dataPath))
            {
                try
                {
                    //初始化序列器
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Converters.Add(new JavaScriptDateTimeConverter());
                    serializer.NullValueHandling = NullValueHandling.Ignore;

                    //构建Json.net的读取流
                    JsonReader reader = new JsonTextReader(sr);
                    //对读取出的reader流进行反序列化，并装载到模型中
                    gongsis = serializer.Deserialize<GongsiNameList>(reader);
                    return gongsis;
                }catch(Exception ex)
                {
                    throw ex;
                }
            }
        }

        public void WriteData(GongsiNameList gongsis)
        {
            using(StreamWriter sw = new StreamWriter(dataPath))
            {
                try
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Converters.Add(new JavaScriptDateTimeConverter());
                    //serializer.NullValueHandling = NullValueHandling.Ignore;

                    //构建Json.net写入流
                    JsonWriter writer = new JsonTextWriter(sw);
                    //模型序列化并写入Json.net的JsonWriter流中
                    serializer.Serialize(writer, gongsis);
                    writer.Close();
                    sw.Close();
                }catch(Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
