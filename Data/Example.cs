//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace mwbFairtradeScript.Data {
//    public class Example {
//        public void CalculateProfit() {
//            Excel.Worksheet datasheet;
//            Excel.Worksheet resultSheet;

//            double sumVolumeBuy = 0;
//            double sumVolumeSell = 0;
//            double sumAmountBuy = 0;
//            double sumAmountSell = 0;

//            double nettingProfit2 = 0;
//            double closingProfit2 = 0;
//            double closingProfit3 = 0;
//            double totalVolume = 0;
//            string myDate = "";
//            string theDate = "";
//            double amount = 0;
//            double price = 0;
//            double lastPrice = 0;
//            string side = "";
//            double averagePriceBuy = 0;
//            double averagePriceSell = 0;
//            string dateBefore = "";
//            long closingCounter = 0;
//            long orderCounter = 0;
//            string myISIN = "";
//            long maxVolume = 20000;



//            int lastRow = datasheet.Range["A500000"].End[Excel.XlDirection.xlUp].Row;
//            int lastRow2 = resultSheet.Range["A500000"].End[Excel.XlDirection.xlUp].Row;
//            //array of isin
//            var isinArray = (object[,])resultSheet.Range["A2:A" + lastRow2].Value;
//            //array of isin
//            var myArray = (object[,])datasheet.Range["A2:G" + lastRow].Value;

//            for (int t = 1; t <= lastRow - 1; t++) {
//                myISIN = isinArray[t - 1, 1].ToString();
//                myDate = "";
//                amount = 0;
//                price = 0;
//                lastPrice = 0;
//                dateBefore = "";
//                sumVolumeBuy = 0;
//                sumVolumeSell = 0;
//                sumAmountBuy = 0;
//                sumAmountSell = 0;
//                averagePriceSell = 0;
//                averagePriceBuy = 0;

//                for (int i = 2; i <= lastRow - 1; i++) {
//                    string ISIN = myArray[i, 7].ToString();
//                    theDate = myArray[i, 1].ToString();

//                    //если дата поменялась
//                    if (dateBefore != "" && theDate != "" && dateBefore != theDate) {
//                        if (sumVolumeBuy > 0 || sumVolumeSell > 0) {
//                            if (sumVolumeBuy > sumVolumeSell) {
//                                nettingProfit2 += (sumAmountSell * (averagePriceBuy - averagePriceSell));
//                                lastPrice = LastBuyPrice(dateBefore, myISIN);
//                                if (lastPrice > 0 && averagePriceBuy != 0)
//                                    closingProfit3 += ((sumAmountBuy - sumAmountSell) * (averagePriceBuy - lastPrice));
//                            }

//                            if (sumVolumeSell > sumVolumeBuy) {
//                                nettingProfit2 += (sumAmountBuy * (averagePriceBuy - averagePriceSell));
//                                lastPrice = LastSellPrice(dateBefore, myISIN);
//                                if (lastPrice > 0 && averagePriceBuy != 0)
//                                    closingProfit3 += ((sumAmountSell - sumAmountBuy) * (lastPrice - averagePriceSell));
//                            }

//                            if (sumVolumeSell == sumVolumeBuy)
//                                nettingProfit2 += (sumAmountBuy * (averagePriceBuy - averagePriceSell));

//                            closingCounter++;
//                            sumVolumeBuy = 0;
//                            sumVolumeSell = 0;
//                            sumAmountBuy = 0;
//                            sumAmountSell = 0;
//                            averagePriceSell = 0;
//                            averagePriceBuy = 0;
//                        }
//                    }

//                    //если дата та же и isin такой же
//                    if (ISIN == myISIN) {
//                        orderCounter++;
//                        myDate = myArray[i, 1].ToString();
//                        amount = double.Parse(myArray[i, 3].ToString());
//                        price = double.Parse(myArray[i, 4].ToString());
//                        double volume = double.Parse(myArray[i, 5].ToString());
//                        side = myArray[i, 6].ToString();
//                        totalVolume += volume;

//                        if (side == "BUY") {
//                            sumAmountBuy += amount;
//                            sumVolumeBuy += volume;
//                            averagePriceBuy = ((1 - (amount / sumAmountBuy)) * averagePriceBuy) + ((amount / sumAmountBuy) * price);
//                        }

//                        if (side == "SELL") {
//                            sumAmountSell += amount;
//                            sumVolumeSell += volume;
//                            averagePriceSell = ((1 - (amount / sumAmountSell)) * averagePriceSell) + ((amount / sumAmountSell) * price);
//                        }

//                        if (Math.Abs(sumVolumeBuy - sumVolumeSell) > maxVolume) {
//                            if (sumVolumeBuy > sumVolumeSell) {
//                                nettingProfit2 += (sumAmountSell * (averagePriceBuy - averagePriceSell));
//                                if (price > 0 && averagePriceBuy != 0)
//                                    closingProfit2 += ((sumAmountBuy - sumAmountSell) * (averagePriceBuy - price));
//                            }

//                            if (sumVolumeSell > sumVolumeBuy) {
//                                nettingProfit2 += (sumAmountBuy * (averagePriceBuy - averagePriceSell));
//                                if (price > 0 && averagePriceSell > 0)
//                                    closingProfit2 += ((sumAmountSell - sumAmountBuy) * (price - averagePriceSell));
//                            }

//                            sumVolumeBuy = 0;
//                            sumVolumeSell = 0;
//                            sumAmountBuy = 0;
//                            sumAmountSell = 0;
//                            averagePriceSell = 0;
//                            averagePriceBuy = 0;
//                            closingCounter++;
//                        }

//                        dateBefore = myDate;
//                        myDate = "";
//                        amount = 0;
//                        price = 0;
//                    }
//                    theDate = "";
//                }

//                resultSheet.Cells[t + 1, 2] = totalVolume;
//                resultSheet.Cells[t + 1, 3] = closingCounter;
//                resultSheet.Cells[t + 1, 4] = orderCounter;
//                resultSheet.Cells[t + 1, 5] = nettingProfit2;
//                resultSheet.Cells[t + 1, 6] = closingProfit2;
//                resultSheet.Cells[t + 1, 7] = closingProfit3;

//                nettingProfit2 = 0;
//                closingProfit2 = 0;
//                closingProfit3 = 0;
//                closingCounter = 0;
//                orderCounter = 0;
//                totalVolume = 0;
//            }
//        }

//        private double LastBuyPrice(string dateBefore, string myISIN) {
//            // Implementation for LastBuyPrice
//            return 0; // Placeholder
//        }

//        private double LastSellPrice(string dateBefore, string myISIN) {
//            // Implementation for LastSellPrice
//            return 0; // Placeholder
//        }
//    }
//}
