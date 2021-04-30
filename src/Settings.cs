using System.IO;
using System.Xml.Serialization;

namespace Kusaanko.Bvets.DenshadeGoInterface
{
    public class Settings
    {
        private const string filename = "Kusaanko.DenshadeGoInput.xml";
        private string directory = string.Empty;

        private int[] buttonS = { -2, 0 };
        private int[] buttonP = { -2, 1 };
        private int[] buttonA = { -2, 2 };
        private int[] buttonB = { -2, 3 };
        private int[] buttonC = { -2, 4 };
        private string controllerName = "JC-PS101U";

        public void SaveToXml()
        {
            try
            {
                if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

                // XmlSerializerオブジェクトを作成
                // 書き込むオブジェクトの型を指定する
                XmlSerializer serializer = new XmlSerializer(typeof(Settings));

                using (FileStream fs = new FileStream(Path.Combine(directory, filename), FileMode.Create)) // ファイルを開く
                {
                    serializer.Serialize(fs, this); // シリアル化し、XMLファイルに保存する
                }
            }
            catch
            {
            }
        }

        public static Settings LoadFromXml(string directory)
        {
            Settings settings;
            try
            {
                // XmlSerializerオブジェクトの作成
                XmlSerializer serializer = new XmlSerializer(typeof(Settings));

                using (FileStream fs = new FileStream(Path.Combine(directory, filename), FileMode.Open))
                {
                    settings = (Settings)serializer.Deserialize(fs); // XMLファイルから読み込み、逆シリアル化する
                }
            }
            catch
            {
                settings = new Settings();
            }

            settings.directory = directory;

            return settings;
        }

        public int[] ButtonS
        {
            get { return buttonS; }
            set { buttonS = value; }
        }

        public int[] ButtonP
        {
            get { return buttonP; }
            set { buttonP = value; }
        }

        public int[] ButtonA
        {
            get { return buttonA; }
            set { buttonA = value; }
        }

        public int[] ButtonB
        {
            get { return buttonB; }
            set { buttonB = value; }
        }

        public int[] ButtonC
        {
            get { return buttonC; }
            set { buttonC = value; }
        }

        public string ControllerName
        {
            get { return controllerName; }
            set { controllerName = value; }
        }

        public int Button0 = 0;
        public int Button1 = 1;
        public int Button2 = 2;
        public int Button3 = 3;
        public int Button4 = 4;
        public int Button5 = 5;
        public int Button6 = 6;
        public int Button7 = 7;
        public int Button8 = 8;
        public int Button9 = 9;
        public bool safeLeverMove = false;
    }
}
