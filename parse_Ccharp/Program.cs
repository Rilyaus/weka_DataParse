using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace parse_Ccharp
{
	class Program
	{
		static void Main(string[] args)
		{
			string dirPath = @"c:\Users\Rilyaus-nLab\Downloads\EC_LIB_2016\DATA";

            Console.WriteLine("\n\n1 ) Default    2) Weka 4attr    3) Weka 6attr");
            Console.Write("\nInput Option : ");
            var option = Console.ReadLine();
            
            switch(option) {
                case "1":
                    parseDefault(dirPath);
                    break;
                case "2":
                    parseWeka4(dirPath);
                    break;
                case "3":
                    parseWeka6(dirPath);
                    break;
                default :
                    break;
            }
		}
        static void parseWeka4(string dirPath) {
            var parsePath = dirPath + @"\Parse";
            DirectoryInfo di = new DirectoryInfo(parsePath);

            Console.WriteLine("\nWeka Path : " + di.FullName);      
            
            foreach( var item in di.GetFiles() ) {
                FileStream divLat = File.Create(parsePath + @"\attr4\lat4_" + item.Name.Substring(0, item.Name.Length-4) + ".arff");
                FileStream divLon = File.Create(parsePath + @"\attr4\lon4_" + item.Name.Substring(0, item.Name.Length-4) + ".arff");

                StreamReader sr = new StreamReader(parsePath + @"\" + item.Name);
                StreamWriter wLat = new StreamWriter(divLat);
                StreamWriter wLon = new StreamWriter(divLon);

                //Default Settings
                wLon.WriteLine("@relation whatever\n");
                wLon.WriteLine("@attribute obs_lon numeric");
                wLon.WriteLine("@attribute velocity_U numeric");
                wLon.WriteLine("@attribute velocity_V numeric");
                wLon.WriteLine("@attribute wind_U numeric");
                wLon.WriteLine("@attribute wind_V numeric");
                wLon.WriteLine("\n\n@data");

                wLat.WriteLine("@relation whatever\n");
                wLat.WriteLine("@attribute obs_lat numeric");
                wLat.WriteLine("@attribute velocity_U numeric");
                wLat.WriteLine("@attribute velocity_V numeric");
                wLat.WriteLine("@attribute wind_U numeric");
                wLat.WriteLine("@attribute wind_V numeric");
                wLat.WriteLine("\n\n@data");

                while (sr.Peek() >= 0) {
                    var parts = sr.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    double convLon = Math.Round(double.Parse(parts[4]), 7);
                    double convLat = Math.Round(double.Parse(parts[5]), 7);

                    wLon.WriteLine(convLon + " , " + parts[8] + " , " + parts[9] + " , " + parts[10] + " , " + parts[11]);
                    wLat.WriteLine(convLat + " , " + parts[8] + " , " + parts[9] + " , " + parts[10] + " , " + parts[11]);
                }
                wLat.Close();
                wLon.Close();
                sr.Close();
                divLat.Close();
                divLon.Close();
            }
        }

        static void parseWeka6(string dirPath) {
            var parsePath = dirPath + @"\Parse";
            DirectoryInfo di = new DirectoryInfo(parsePath);

            Console.WriteLine("\nWeka Path : " + di.FullName);

            foreach (var item in di.GetFiles()) {
                FileStream divLat = File.Create(parsePath + @"\attr6\lat4_" + item.Name);
                FileStream divLon = File.Create(parsePath + @"\attr6\lon4_" + item.Name);

                StreamReader sr = new StreamReader(parsePath + @"\" + item.Name);
                StreamWriter wLat = new StreamWriter(divLat);
                StreamWriter wLon = new StreamWriter(divLon);

                //Default Settings
                wLon.WriteLine("@relation whatever\n");
                wLon.WriteLine("@attribute obs_lon numeric");
                wLon.WriteLine("@attribute model_lon numeric");
                wLon.WriteLine("@attribute model_lat numeric");
                wLon.WriteLine("@attribute velocity_U numeric");
                wLon.WriteLine("@attribute velocity_V numeric");
                wLon.WriteLine("@attribute wind_U numeric");
                wLon.WriteLine("@attribute wind_V numeric");
                wLon.WriteLine("\n\n@data");

                wLat.WriteLine("@relation whatever\n");
                wLat.WriteLine("@attribute obs_lat numeric");
                wLat.WriteLine("@attribute model_lon numeric");
                wLat.WriteLine("@attribute model_lat numeric");
                wLat.WriteLine("@attribute velocity_U numeric");
                wLat.WriteLine("@attribute velocity_V numeric");
                wLat.WriteLine("@attribute wind_U numeric");
                wLat.WriteLine("@attribute wind_V numeric");
                wLat.WriteLine("\n\n@data");

                while (sr.Peek() >= 0) {
                    var parts = sr.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    double convLon = Math.Round(double.Parse(parts[4]), 7);
                    double convLat = Math.Round(double.Parse(parts[5]), 7);

                    double convMLon = Math.Round(double.Parse(parts[6]), 7);
                    double convMLat = Math.Round(double.Parse(parts[7]), 7);

                    wLon.WriteLine(convLon + " , " + convMLon + " , " + convMLat + " , " + parts[8] + " , " + parts[9] + " , " + parts[10] + " , " + parts[11]);
                    wLat.WriteLine(convLat + " , " + convMLon + " , " + convMLat + " , " + parts[8] + " , " + parts[9] + " , " + parts[10] + " , " + parts[11]);
                }
                wLat.Close();
                wLon.Close();
                sr.Close();
                divLat.Close();
                divLon.Close();
            }
        }

        static void parseDefault(string dirPath) {
            DirectoryInfo di = new DirectoryInfo(dirPath);

            Console.WriteLine("Current Path : " + di.FullName);

            foreach (var item in di.GetFiles()) {
                FileStream fTrain = File.Create(dirPath + @"\Parse\absTrain_" + item.Name);
                FileStream fTest = File.Create(dirPath + @"\Parse\absTest_" + item.Name);

                StreamReader sr = new StreamReader(dirPath + @"\" + item.Name);
                StreamWriter sTrain = new StreamWriter(fTrain);
                StreamWriter sTest = new StreamWriter(fTest);
                List<String> oParts = new List<string>();

                int count = 0;
                int divideCount = 0;

                int lineCount = File.ReadAllLines(dirPath + @"\" + item.Name).Count() - 2;
                int trainCount = (lineCount / 3) * 2 + (lineCount % 3);

                while (sr.Peek() >= 0) {
                    count++;
                    var parts = sr.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    if (count > 2 && divideCount < trainCount) {
                        for (int i = 0; i < parts.Length; i++) {
                            if (i < 4) {
                                sTrain.Write("{0,3} ", parts[i]);
                            } else if (i == 4 || i == 5) { // 측정 위도, 경도 값 변화량 계산
                                double temp = Math.Round(double.Parse(parts[i]), 7); // Absolute
                                                                                     //double temp = Math.Round(double.Parse(parts[i]) - double.Parse(oParts[i]), 7); // Relative
                                sTrain.Write("{0,11:F6} ", temp);
                            } else if (i == 6 || i == 7) { // 모델 예측 위도, 경도 값 변화량 계산
                                double temp = Math.Round(double.Parse(parts[i]), 7); // Absolute
                                                                                     //double temp = Math.Round(double.Parse(parts[i]) - double.Parse(oParts[i]), 7); // Relative
                                sTrain.Write("{0,11:F6} ", temp);
                            } else if (i > 7) {
                                sTrain.Write("{0,8} ", oParts[i]);
                            } else {
                                sTrain.Write(parts[i] + " ");
                            }
                        }
                        sTrain.WriteLine();
                        divideCount++;
                    } else if (divideCount >= trainCount) {
                        for (int i = 0; i < parts.Length; i++) {
                            if (i < 4) {
                                sTest.Write("{0,3} ", parts[i]);
                            } else if (i == 4 || i == 5) { // 측정 위도, 경도 값 변화량 계산
                                double temp = Math.Round(double.Parse(parts[i]), 7); // Absolute
                                                                                     //double temp = Math.Round(double.Parse(parts[i]) - double.Parse(oParts[i]), 7); // Relative
                                sTest.Write("{0,11:F6} ", temp);
                            } else if (i == 6 || i == 7) { // 모델 예측 위도, 경도 값 변화량 계산
                                double temp = Math.Round(double.Parse(parts[i]), 7); // Absolute
                                                                                     //double temp = Math.Round(double.Parse(parts[i]) - double.Parse(oParts[i]), 7); // Relative
                                sTest.Write("{0,11:F6} ", temp);
                            } else if (i > 7) {
                                sTest.Write("{0,8} ", oParts[i]);
                            } else {
                                sTest.Write(parts[i] + " ");
                            }
                        }
                        sTest.WriteLine();
                        divideCount++;
                    }
                    oParts.Clear();
                    oParts.AddRange(parts);
                }
                sr.Close();
                sTrain.Close();
                sTest.Close();
                fTrain.Close();
                fTest.Close();
            }
        }
	}
}
