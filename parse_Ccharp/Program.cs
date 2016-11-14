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
			string dirPath = @"c:\users\rilyaus\downloads\EC_LIB_2016\DATA";

			DirectoryInfo di = new DirectoryInfo(dirPath);

			Console.WriteLine("Current Path : " + di.FullName);

			foreach(var item in di.GetFiles()) {
				FileStream fsa = File.Create(dirPath + @"\Parse\parse_" + item.Name);

				StreamReader sr = new StreamReader(dirPath + @"\" + item.Name);
				StreamWriter sw = new StreamWriter(fsa);
				List<String> oParts = new List<string>();

				int count = 0;

				while( sr.Peek() >= 0 ) {
					count++;
					var parts = sr.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

					if( count > 2 ) {
						for( int i=0 ; i<parts.Length ; i++ ) {
							if( i < 4 ) {
								sw.Write("{0,3} ", parts[i]);
							} else if( i == 4 || i == 5 ) { // 측정 위도, 경도 값 변화량 계산
								double temp = Math.Round(double.Parse(parts[i]) - double.Parse(oParts[i]), 7);
								sw.Write("{0,11:F6} ", temp);
							} else if( i == 6 || i == 7 ) { // 모델 예측 위도, 경도 값 변화량 계산
								double temp = Math.Round(double.Parse(parts[i]) - double.Parse(oParts[i-2]), 7);
								sw.Write("{0,11:F6} ", temp);
							} else if( i > 7 ) {
								sw.Write("{0,8} ", oParts[i]);
							} else {
								sw.Write(parts[i] + " ");
							}
						}
						sw.WriteLine();
					}

					//if( count == 10 ) break;

					oParts.Clear();
					oParts.AddRange(parts);
				}
				sr.Close();
				sw.Close();
				fsa.Close();
			}

		}
	}
}
