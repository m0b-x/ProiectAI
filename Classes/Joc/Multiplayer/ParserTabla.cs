using System;
using System.Diagnostics;
using System.Linq;

namespace ProiectVolovici.Classes.Joc.Multiplayer
{
	public class ParserTabla
	{
		private int _liniiDecodificate;
		private int _coloaneDecodificate;

		public ParserTabla(int liniiDecodificate, int coloaneDecodificate)
		{
			_liniiDecodificate = liniiDecodificate;
			_coloaneDecodificate = coloaneDecodificate;
		}

		public string CodificareMutare(Pozitie pozitieInitiala, Pozitie pozitieFinala)
		{
			string mutareString = "{" +
									pozitieInitiala.Linie + "," +
									pozitieInitiala.Coloana + "," +
									pozitieFinala.Linie + "," +
									pozitieFinala.Coloana +
								   "}";
			return mutareString;
		}

		public Tuple<Pozitie, Pozitie> DecodificareMutare(string mutare)
		{
			mutare = mutare.Replace("{", " ");
			mutare = mutare.Replace("}", " ");
			int[] vectorPozitiiInt = mutare.Split(',').Select(int.Parse).ToArray();

			return new Tuple<Pozitie, Pozitie>(new Pozitie(ConstantaTabla.NrLinii - 1 - vectorPozitiiInt[0], ConstantaTabla.NrColoane - 1 - vectorPozitiiInt[1]),
											   new Pozitie(ConstantaTabla.NrLinii - 1 - vectorPozitiiInt[2], ConstantaTabla.NrColoane - 1 - vectorPozitiiInt[3]));
		}

		private static T[,] ConvertesteJaggedIn2D<T>(T[][] source)
		{
			try
			{
				int FirstDim = source.Length;
				int SecondDim = source.GroupBy(row => row.Length).Single().Key; // throws InvalidOperationException if source is not rectangular

				var result = new T[FirstDim, SecondDim];
				for (int i = 0; i < FirstDim; ++i)
					for (int j = 0; j < SecondDim; ++j)
						result[i, j] = source[i][j];

				return result;
			}
			catch (InvalidOperationException)
			{
				throw new InvalidOperationException("The given jagged array is not rectangular.");
			}
		}

		public string CodificareTablaSiAspect(int[][] matriceTabla, Aspect aspect)
		{
			int[,] matrice2D = new int[ConstantaTabla.NrLinii, ConstantaTabla.NrColoane];
			matrice2D = ConvertesteJaggedIn2D(matriceTabla);
			var str = /*"mesajInceput" + */ string.Join("", matrice2D.OfType<int>()
											.Select((value, index) => new { value, index })
											.GroupBy(linie => linie.index / matrice2D.GetLength(1))
											.Select(linie => $"{{{string.Join(" ", linie.Select(linie => linie.value))}}}"));

			return $"{(int)aspect}{str}";
		}

		public int[][] DecodificareTablaSiAspect(string stringPrimit, out Aspect aspect)
		{
			aspect = (Aspect)(stringPrimit[0] - '0');
			stringPrimit = stringPrimit.Remove(0, 1);

			stringPrimit = stringPrimit.Replace("}{", " ");

			stringPrimit = stringPrimit.Substring(1, stringPrimit.Length - 2);

			Debug.WriteLine(stringPrimit);
			int[] vectorAuxiliar = stringPrimit.Split(' ').Select(int.Parse).ToArray();

			int[][] matriceReturnata = new int[_liniiDecodificate][];
			for (int i = 0; i < _liniiDecodificate; i++)
				matriceReturnata[i] = new int[_coloaneDecodificate];

			int contorElemente = 0;
			for (int linie = 0; linie < _liniiDecodificate; linie++)
			{
				for (int coloana = 0; coloana < _coloaneDecodificate; coloana++)
				{
					matriceReturnata[linie][coloana] = vectorAuxiliar[contorElemente++];
				}
			}
			return (matriceReturnata);
		}
	}
}