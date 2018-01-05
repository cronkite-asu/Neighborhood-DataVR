using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Csv reader
/// 	Class for reading the data from the csv file and escaping the rows with special characters
/// 	return the appropridate columns of the row by escaping characters
/// </summary>
public sealed class CsvReader : System.IDisposable
{
	public CsvReader( string fileName ) : this( new FileStream(fileName, FileMode.Open, FileAccess.Read ) )
	{
	}

	public CsvReader( Stream stream )
	{
		__reader = new StreamReader( stream );
	}

	/// <summary>
	/// Csvs the column split processor.
	/// 	Splits the line in csv format by escaping certain characters
	/// </summary>
	/// <returns>The column split processor.</returns>
	/// <param name="line">Line.</param>
	public static List<string> csvColumnSplitProcessor(string line)
	{
		string[] values = rexCsvSplitter.Split( line );
		List<string> colValues = new List<string> ();
		
		for (int i = 0; i < values.Length; i++) {
			colValues.Add (Csv.Unescape (values [i]));
		}
		return colValues;
	}

	public System.Collections.IEnumerable RowEnumerator
	{
		get {
			if ( null == __reader )
				throw new System.ApplicationException( "I can't start reading without CSV input." );

			__rowno = 0;
			string sLine;
			string sNextLine;

			while ( null != ( sLine = __reader.ReadLine() ) )
			{
				while ( rexRunOnLine.IsMatch( sLine ) && null != ( sNextLine = __reader.ReadLine() ) )
					sLine += "\n" + sNextLine;

				__rowno++;
				string[] values = rexCsvSplitter.Split( sLine );

				for ( int i = 0; i < values.Length; i++ )
					values[i] = Csv.Unescape( values[i] );

				yield return values;
			}

			__reader.Close();
		}
	}

	public long RowIndex { get { return __rowno; } }

	public void Dispose()
	{
		if ( null != __reader ) __reader.Dispose();
	}

	//============================================


	private long __rowno = 0;
	private TextReader __reader;
	private static Regex rexCsvSplitter = new Regex( @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))" );
	private static Regex rexRunOnLine = new Regex( @"^[^""]*(?:""[^""]*""[^""]*)*""[^""]*$" );
}

public static class Csv
{
	public static string Escape( string s )
	{
		if ( s.Contains( QUOTE ) )
			s = s.Replace( QUOTE, ESCAPED_QUOTE );

		if ( s.IndexOfAny( CHARACTERS_THAT_MUST_BE_QUOTED ) > -1 )
			s = QUOTE + s + QUOTE;

		return s;
	}

	public static string Unescape( string s )
	{
		if ( s.StartsWith( QUOTE ) && s.EndsWith( QUOTE ) )
		{
			s = s.Substring( 1, s.Length - 2 );

			if ( s.Contains( ESCAPED_QUOTE ) )
				s = s.Replace( ESCAPED_QUOTE, QUOTE );
		}

		return s;
	}


	private const string QUOTE = "\"";
	private const string ESCAPED_QUOTE = "\"\"";
	private static char[] CHARACTERS_THAT_MUST_BE_QUOTED = { ',', '"', '\n' };
}