﻿//////////////////////////////////////////////////////////////////////////////////////////
// GPdotNET - Genetic Programming Tool                                                  //
// Copyright 2006-2017 Bahrudin Hrnjica                                                 //
//                                                                                      //
// This code is free software under the MIT License                                     //
// See license section of  http://github.com/bhrnjica/gpdotnet/blob/master/license.md  //
//                                                                                      //
// Bahrudin Hrnjica                                                                     //
// bhrnjica@hotmail.com                                                                 //
// Bihac,Bosnia and Herzegovina                                                         //
// http://bhrnjica.wordpress.com                                                        //
//////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Text;
using GPdotNet.Interfaces;
using System.Collections.Generic;

namespace GPdotNet.Core
{
    /// <summary>
    /// Implement Sum of Square Error fitness function
    /// </summary>
    public class SEFitness : IFitness
    {
         IData Data;
        public SEFitness(IData data)
        {
            Data = data;
        }
        public float Evaluate(IChromosome ch, IParameters param, int it)
        {
            var iData = GetData();
            var expTree = ((Chromosome)ch).expressionTree;

            double fitness = 0;
            double rawFitness = 0.0;
            double y;

            //index of output parameter

            for (int i = 0; i < iData.RowCount; i++)
            {
                var rowInputData = iData.GetDataInputRow(i);

                //evaluate the function against each rowData
                y = expTree.Evaluate(rowInputData, param);

                // check for correct numeric value
                if (double.IsNaN(y) || double.IsInfinity(y))
                    return float.NaN;

                //Calculate square error
                var ae = y - iData.GetRowOutput(i);
                rawFitness += (ae * ae);
            }

            if (double.IsNaN(rawFitness) || double.IsInfinity(rawFitness))
                fitness = float.NaN;
            else//Root of sum of square error
                fitness = ((1.0 / (1.0 + rawFitness)) * 1000.0);

            return (float)Math.Round(fitness, 2);
        }

        public IData GetData()
        {
            return Data;
        }
    }
}
