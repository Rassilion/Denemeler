using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ForgeBot
{
    sealed class ItemCollection
    {
        //Singleton
        private static readonly ItemCollection m_Instance = new ItemCollection();

        //General
        private Dictionary<string, Item> m_Items = new Dictionary<string, Item>();

        //Struct
        public struct Item
        {
            /// <summary>
            /// Whether we're about to make first adjustment
            /// </summary>
            public bool ReadyForTheFirstAdjustment
            {
                get { return !LastClickWasSuccessful && YStepping == 0; }
            }

            private const int AbsoluteYMinimum = 30;
            private const int AbsoluteYMaximum = 400;

            public int m_OffsetX, m_OffsetY;
            public string timestamp;
            public int YStepping;
            public bool LastClickWasSuccessful;

            public Item(int p_OffsetX, int p_OffsetY, string p_Timestamp,int yStepping,bool lastClickSuccessful) 
            {
                m_OffsetX = p_OffsetX;
                m_OffsetY = p_OffsetY;
                timestamp = p_Timestamp;
                YStepping = yStepping;
                LastClickWasSuccessful = lastClickSuccessful;
                if (LastClickWasSuccessful)
                    YStepping = 0;
            }

            /// <summary>
            /// When we clicked and popup opened
            /// </summary>
            public void ClickSuccessful(bool wasSuccess)
            {
                LastClickWasSuccessful = wasSuccess;
                if(wasSuccess)
                    YStepping = 0;
            }

            /// <summary>
            /// Calculate new Y offset for click if last click was failure
            /// </summary>
            public void CalculateNewTrialCoordinates()
            {
                if (LastClickWasSuccessful)
                    return;

                if(YStepping==0)
                {
                    // we enter if we are here for the first time (last time click was ok, now it is not anymore)
                    YStepping = -Math.Abs(ApplicationSettings.AutoSearchYStepping);
                    if (m_OffsetY < AbsoluteYMinimum)
                        YStepping = Math.Abs(ApplicationSettings.AutoSearchYStepping);
                }
                else
                {
                    if (m_OffsetY < AbsoluteYMinimum)
                        YStepping = Math.Abs(ApplicationSettings.AutoSearchYStepping);
                    if (m_OffsetY > AbsoluteYMaximum)
                        YStepping = -(Math.Abs(ApplicationSettings.AutoSearchYStepping));
                }
                m_OffsetY += YStepping;
            }
        }

        public static string MakePoint(int x,int y)
        {
            return string.Format("{0:0000}{1:0000}",x,y);
        }

//-->Constructor

        private ItemCollection()
        {

        }

//-->Attributes

        //Singleton
        public static ItemCollection Instance
        {
            get
            {
                return m_Instance;
            }
        }

        //General
        public Dictionary<string, Item> Items
        {
            get { return m_Items; }
            set { m_Items = value; }
        }

        public static string BuildCoordinateData()
        {
            var l_Data = new StringBuilder();

            PurgeOldItems();

            // Loop over pairs with foreach
            foreach (KeyValuePair<string, ItemCollection.Item> pair in ItemCollection.Instance.Items)
            {
                l_Data.AppendFormat("{0},{1},{2},{3},{4},{5};", pair.Key, pair.Value.m_OffsetX, pair.Value.m_OffsetY, pair.Value.timestamp, pair.Value.YStepping, pair.Value.LastClickWasSuccessful);
            }
            return l_Data.ToString();
        }

        /// <summary>
        /// Delete items older than 30 days
        /// </summary>
        private static void PurgeOldItems()
        {
            var now = DateTime.Now;
            var keysToDelete = new List<string>();
            foreach (KeyValuePair<string, ItemCollection.Item> pair in ItemCollection.Instance.Items)
            {
                var ts = new TimeSpan(now.Ticks - long.Parse(pair.Value.timestamp));
                if(ts.TotalDays>30)
                    keysToDelete.Add(pair.Key);
            }

            foreach (var key in keysToDelete)
            {
                ItemCollection.Instance.Items.Remove(key);
            }
        }

        public static void LoadInstanceFromCoordinateData(string settingsCoordinateData)
        {
            try
            {
                ItemCollection l_ItemCollection = ItemCollection.Instance;
                string l_CoordinateString;//Format xxxxyyyy
                string[] l_CoordinatesData = settingsCoordinateData.Split(';');
                string[] l_CoordinateData = l_CoordinatesData[0].Split(',');

                for (int i = 0; i < l_CoordinatesData.Length; i++)
                {
                    ItemCollection.Item l_Item = new ItemCollection.Item();
                    l_CoordinateData = l_CoordinatesData[i].Split(',');
                    if (l_CoordinateData.Length >= 6)
                    {
                        l_CoordinateString = l_CoordinateData[0];
                        if (l_ItemCollection.Items.TryGetValue(l_CoordinateString, out l_Item))
                        {
                            //Item already loaded
                        }
                        else
                        {
                            //New item
                            l_ItemCollection.Items[l_CoordinateString] = new ItemCollection.Item(int.Parse(l_CoordinateData[1]), int.Parse(l_CoordinateData[2]), l_CoordinateData[3], int.Parse(l_CoordinateData[4]), bool.Parse(l_CoordinateData[5]));
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        public static void EraseAllItems()
        {
            ItemCollection.Instance.Items.Clear();
        }
    }
}
