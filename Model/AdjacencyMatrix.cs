﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GraphVisualisationTool.Model
{
    class AdjacencyMatrix : FileHandlerInterface
    {
        public List<List<T>> ParseFile<T>(string filename)
        {
            List <List<bool>> matrix = new List<List<bool>>();
            StreamReader reader = File.OpenText(filename);

            string line;
            int columns = 0,
                rows = 0;

            //read line
            while ((line = reader.ReadLine()) != null)
            {
                //split by whitespace
                string[] items = line.Split(',');
                //convert to integers
                List<bool> convertedItems = new List<bool>();
                foreach (var integer in items)
                {
                    int item;
                    if (FileGlobalVars.getInstance().TryParseInt32(integer, out item) != -1 && (item == 0 || item == 1))
                        convertedItems.Add(item == 0 ? false : true);
                }
                ++rows;
                if (!(items.Length > 1))
                {
                    reader.Close();
                    throw new Exception($"Row {rows} is corrupted!");
                }

                if (rows == 1)
                {
                    matrix.Add(convertedItems.ToList());
                    //columns constant integer is initiliazed
                    columns = convertedItems.Count;
                }
                else if (convertedItems.Count == columns)
                {
                    matrix.Add(convertedItems.ToList());
                }
                else
                {
                    reader.Close();
                    throw new Exception($"Row #{rows} is corrupted!");
                }
            }
            reader.Close();
            if (columns != rows)
            {
                if (rows < columns)
                    throw new Exception("columns is bigger than rows");
                else
                    throw new Exception("rows is bigger than columns");
            }
            for(int i = 0; i < rows ; i++)
            {
                if (matrix[i][i] == true)
                    throw new Exception("Cannot open a directed graph");
                for (int j = 0; j < columns; j++)
                    if (matrix[i][j] != matrix [j][i])
                        throw new Exception("Cannot open a directed graph");
            }
            return (List<List<T>>)Convert.ChangeType(matrix, typeof(List<List<T>>)) ;
        }
    }
}
