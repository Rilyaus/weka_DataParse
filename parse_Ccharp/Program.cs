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

			DirectoryInfo di = new DirectoryInfo(dirPath);

			Console.WriteLine("Current Path : " + di.FullName);

			foreach(var item in di.GetFiles()) {
				FileStream fTrain = File.Create(dirPath + @"\Parse\abs_train_" + item.Name);
                FileStream fTest = File.Create(dirPath + @"\Parse\abs_test_" + item.Name);

				StreamReader sr = new StreamReader(dirPath + @"\" + item.Name);
                StreamWriter sTrain = new StreamWriter(fTrain);
                StreamWriter sTest = new StreamWriter(fTest);
                List<String> oParts = new List<string>();

				int count = 0;

                int lineCount = File.ReadAllLines(dirPath + @"\" + item.Name).Count();
                int trainCount = (lineCount / 3) * 2 + (lineCount % 3);
                
				while( sr.Peek() >= 0 ) {
					count++;
					var parts = sr.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

					if( count > 2 && count < trainCount ) {
						for( int i=0 ; i<parts.Length ; i++ ) {
							if( i < 4 ) {
								sTrain.Write("{0,3} ", parts[i]);
							} else if( i == 4 || i == 5 ) { // 측정 위도, 경도 값 변화량 계산
								double temp = Math.Round(double.Parse(parts[i]), 7); // Absolute
                                                                                     //double temp = Math.Round(double.Parse(parts[i]) - double.Parse(oParts[i]), 7); // Relative
                                sTrain.Write("{0,11:F6} ", temp);
							} else if( i == 6 || i == 7 ) { // 모델 예측 위도, 경도 값 변화량 계산
                                double temp = Math.Round(double.Parse(parts[i]), 7); // Absolute
                                                                                     //double temp = Math.Round(double.Parse(parts[i]) - double.Parse(oParts[i]), 7); // Relative
                                sTrain.Write("{0,11:F6} ", temp);
							} else if( i > 7 ) {
                                sTrain.Write("{0,8} ", oParts[i]);
							} else {
                                sTrain.Write(parts[i] + " ");
							}
						}
                        sTrain.WriteLine();
					} else if( count >= trainCount) {
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
                    }

					//if( count == 10 ) break;

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
