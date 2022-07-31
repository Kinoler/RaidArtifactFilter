using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RaidFilterUI.Forms
{
    /// <summary>
    /// Логика взаимодействия для ArtifactControl.xaml
    /// </summary>
    public sealed partial class ArtifactControl : UserControl
    {
        public ArtifactControl()
        {
            InitializeComponent();
        }
        
        public void Init(ArtifactControlModel model)
        {
            this.LevelLabel.Content = model.Level > 0 ? $"+{model.Level}" : "";
            this.StarLabel.Content = $"{model.Rank}";
            var color = Color.FromRgb(model.Rarity.R, model.Rarity.G, model.Rarity.B);
            this.RarityBorder.BorderBrush = new SolidColorBrush(color);
            this.ArtifactImage.Source = ImageCache.Instance.GetImage(model.ImgName);

            this.MainStatName.Content = model.Stats[0].Name;
            this.MainStatValue.Content = model.Stats[0].Value;

            if (model.Stats.Length > 1)
            {
                this.SubStatName1.Content = model.Stats[1].Name;
                this.SubStatValue1.Content = model.Stats[1].Value;
                this.SubStatPower1.Content = model.Stats[1].Power;
            }
            else
            {
                this.SubStatName1.Content = "";
                this.SubStatValue1.Content = "";
                this.SubStatPower1.Content = "";
            }

            if (model.Stats.Length > 2)
            {
                this.SubStatName2.Content = model.Stats[2].Name;
                this.SubStatValue2.Content = model.Stats[2].Value;
                this.SubStatPower2.Content = model.Stats[2].Power;
            }
            else
            {
                this.SubStatName2.Content = "";
                this.SubStatValue2.Content = "";
                this.SubStatPower2.Content = "";
            }

            if (model.Stats.Length > 3)
            {
                this.SubStatName3.Content = model.Stats[3].Name;
                this.SubStatValue3.Content = model.Stats[3].Value;
                this.SubStatPower3.Content = model.Stats[3].Power;
            }
            else
            {
                this.SubStatName3.Content = "";
                this.SubStatValue3.Content = "";
                this.SubStatPower3.Content = "";
            }

            if (model.Stats.Length > 4)
            {
                this.SubStatName4.Content = model.Stats[4].Name;
                this.SubStatValue4.Content = model.Stats[4].Value;
                this.SubStatPower4.Content = model.Stats[4].Power;
            }
            else
            {
                this.SubStatName4.Content = "";
                this.SubStatValue4.Content = "";
                this.SubStatPower4.Content = "";
            }

            /*
            if (!Cache.ContainsKey(model.Rank))
            {
                var gr = new Grid();
                gr.Margin = new Thickness(10, 10, 142, 55);
                this.StarGrid.RowDefinitions.Add(new RowDefinition());
                for (var i = 0; i < model.Rank; i++)
                {
                    var img = new Image()
                    {
                        Source = ImageCache.Instance.GetImage("star.png"),
                        Width = 20,
                        Margin = new Thickness(10 + i * 8, 0, 0, 0),
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top
                    };

                    Grid.SetRow(img, 0);
                    Grid.SetColumn(img, i);
                    this.StarGrid.ColumnDefinitions.Add(new ColumnDefinition());
                    this.StarGrid.Children.Add(img);
                }

             //   Cache.Add(model.Rank, gr);
            }
            //this.StarGrid = Cache[model.Rank];
            */
        }
    }

    public class ArtifactControlModel
    {
        public int Level { get; set; }
        public string ImgName { get; set; }
        public int Rank { get; set; }
        public System.Drawing.Color Rarity { get; set; }
        public StatControlModel[] Stats { get; set; }
    }

    public class StatControlModel
    {
        public StatControlModel(string name, double value, bool absolute, int level = 0, double power = 0)
        {
            Name = level == 0 ? name : $"{name}({level})";
            Value = absolute ? ((int)value).ToString() : $"{(int)(value * 100)}%";
            Power = power != 0 ? $"+{(absolute ? ((int)power).ToString() : (int)(power * 100) + "%")}" : "";
        }

        public string Name { get; set; }
        public string Value { get; set; }
        public string Power { get; set; }
    }
}
