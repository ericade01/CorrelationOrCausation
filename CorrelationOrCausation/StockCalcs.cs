using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorrelationOrCausation
{
    class StockCalcs
    {

        // future price (next price) =  close price , curr time


        public void LineGuess(double currprices, double movingaverage)
        {

            double futureprice = 0d;



            double h = 1d; // this is the static coefficient


            //10 = length of list.
            for (int x = 0; x < 10; x++)
            {
                currprices = (currprices + h * movingaverage);
            }



            // subtract difference of starting and ending price (real ones)
            // subtrade difference of EST starting and EST ending price (ones before and after loop)
            // subtract difference between them. this is absolute error. 


            // relative error not absolute 



        }





        public double GetRelativeError(double currprices, double truefutureprice, double movingaverage, int CurrpriceTime)
        {
            double relativeerror = 0d;

            double predictedfutureprice = currprices;


            double coefficientforhw = 0d;//the w is for blue
                                         //need to compute this before put in
                                         //based on how much lineguess is off by

            //absolute error = predictive minus true price in future


            double h = 1d; // this is the static coefficient





            predictedfutureprice = (currprices + h * movingaverage);






            //(r1)  first portion of relative error = (true future price - current price )  
            //(r2) second portion of relative error = (predicted future price - true future price) 

            //relative error = r1/r2



            //multiply relative error BY H 


            return relativeerror;

        }




        /*
         *   "a quantitiy" which is b_j
         *   algorithm is: 
         * c_j = b_j / B,j
         * 
         * j is data point 
         * c
         * 
         * 
         * 
         * 
         *    polynomial approx. takes data points (list) and takes a number 0 
         *    linear approximate from 0-1
         *    needs to solve for 2 coefficients -
         * 
         *     B = first entry in "a matrix"
         *     denominator
         *     
         *     naumerator = b
         *     
         *     b/B = what we want

           "c1" and "c0" from "top"

        ith neumerator by ith denom = ith coefficiant

        time = t
        price = p

        integral of t p* (some function) = 



         * */


      




    }
}
