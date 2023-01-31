using KFinance.DTOs;

namespace KFinance.Utils
{
    public class Helper
    {
        public Helper() { }
        public string CalculatePercentageChange(List<History> res, RequestDTO requestDTO)
        {
            decimal invested = 0M;
            decimal totalCurrentValue = 0M;
            decimal shares = 0M;
            decimal percentChange = 0M;
            string result = string.Empty;
            if (res is not null)
            {
                var filteredHistory = res.ToList().Where(h => DateTime.Compare(h.Date, requestDTO.Start) >= 0 && DateTime.Compare(h.Date, requestDTO.End) <= 0).ToList();
                filteredHistory.Sort();
                int iterator = 0;
                int buyCount = 0;
                while (true)
                {
                    if (iterator % requestDTO.Frequency == 0)
                    {
                        DateTime date = requestDTO.Start.AddDays(iterator);
                        if (DateTime.Compare(date, filteredHistory[^1].Date) > 0) break;
                        //Get the closet available trading day with price
                        foreach (var item in filteredHistory)
                        {
                            if (DateTime.Compare(item.Date, date) >= 0)
                            {
                                shares += requestDTO.Amount / item.Open;
                                buyCount++;
                                //result += String.Format("{0,-30} - {1, -10} - {2,10} - {3,5}\n", item.Date.Date, Math.Round(item.Open, 4), Math.Round(requestDTO.Amount / item.Open, 4), buyCount);
                                break;
                            }
                        }
                    }
                    iterator++;
                }
                invested = buyCount * requestDTO.Amount;
                totalCurrentValue = shares * filteredHistory[^1].Open;
                percentChange = Math.Round(((totalCurrentValue - invested) / invested), 4);
                result += String.Format("Increase / Decrease : {0:P2}.\n", percentChange);
                result += String.Format("Results: ----------------------------------------------------------------- \n");
                result += String.Format("{0, -20} - {1, 20}\n", "Before", "After");
                result += String.Format("{0, -20} - {1, 20}\n", invested, totalCurrentValue);
            }

            //MachineLearning(res, requestDTO);
            return percentChange.ToString();
        }

        public string MachineLearning(List<History> histories, RequestDTO requestDTO)
        {
            DateTime start = histories[0].Date;
            DateTime end = histories[^1].Date;
            int length = histories[^1].Date.Year - histories[0].Date.Year + 1;
            int[] years = new int[length];
            for (int i = 0; i < length; i++)
            {
                years[i] = start.AddYears(i).Year;
            }
            Dictionary<string, string> records = new(); // "2012 - 2015" : 0.3526
            /*
            int gap = 1;
            while(gap <= length)
            {
                while(start.Year + gap <= end.Year)
                {
                    requestDTO.Start = start;
                    requestDTO.End = start.AddYears(gap);
                    records.Add($"{start.ToString()} - {end.ToString()}", CalculatePercentageChange(histories, requestDTO));
                    //records.Add(new KeyValuePair<string, string>($"{start.ToString()} - {end.ToString()}", CalculatePercentageChange(histories, requestDTO)));
    
                }
                gap++;
            }
            */
            for(int i = 0; i < length; i++)
            {
                for(int j = i + 1; j < length; j++)
                {
                    requestDTO.Start = new DateTime(years[i], 1,1);
                    requestDTO.End = new DateTime(years[j], 1, 1);
                    records.Add($"{requestDTO.Start.Year.ToString()} - {requestDTO.End.Year.ToString()}", CalculatePercentageChange(histories, requestDTO));
                }
            }
            var s = from temp in records orderby Convert.ToDecimal(temp.Value) ascending select temp;
            string result = string.Empty;
            foreach(var item in s)
            {
                result += string.Format("{0, -30} - {1, 10}%\n", item.Key, Math.Round(Convert.ToDecimal(item.Value)*100, 2));
            }
            return result;
        }
    }
}
