using Business_Layer;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.WindowsForms.ToolTips;
using Service_Layer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        Service service = new Service();
        Dictionary<string, State> states;
        List<string> keys;
        List<Color> colors;
        List<Tweet> tweets;
        Dictionary<State, double> statesSentiments;

        public Form1()
        {
            InitializeComponent();
            filesComboBox.SelectedIndexChanged += FilesComboBox_SelectedIndexChanged;
            filesComboBox.SelectedIndexChanged += GetData;
            filesComboBox.SelectedIndexChanged += DrawPolygons;
            radioButton1.CheckedChanged += DrawMarkers;

        }

        private void GMapControl1_Load(object sender, EventArgs e)
        {
            filesComboBox.DataSource = new List<string>
        {
                "cali_tweets2014.txt",
                "movie_tweets2014.txt",
                "family_tweets2014.txt",
                "shopping_tweets2014.txt",
                "snow_tweets2014.txt",
                "texas_tweets2014.txt",
                "weekend_tweets2014.txt"
        }; ;
            gMapControl1.Bearing = 0;
            gMapControl1.CanDragMap = true; //Открываем доступ к манипулированию картой мышью через зажатие правой кнопкой(по умолчанию).
            gMapControl1.DragButton = MouseButtons.Left; //Меняем кнопку манипулирования с правой кнопки(по умолчанию) на левую кнопку мыши.
            gMapControl1.GrayScaleMode = true;
            gMapControl1.MaxZoom = 18; //Устанавливаем максимальное приближение.
            gMapControl1.MinZoom = 2; //Устанавливаем минимальное приближение.
            gMapControl1.Zoom = 4;
            gMapControl1.MouseWheelZoomType = MouseWheelZoomType.MousePositionAndCenter; //Устанавливаем центр приближения/удаления курсор мыши.

            gMapControl1.PolygonsEnabled = true; //Открываем отображение полигонов на карте.
            gMapControl1.MarkersEnabled = true; //Открываем отображение маркеров на карте.
            gMapControl1.NegativeMode = false; //Отказываемся от негативного режима
            gMapControl1.ShowTileGridLines = false; //Скрываем внешнюю сетку карты с заголовками

            gMapControl1.Dock = DockStyle.None; //Закрепляем контрол внутри формы, чтобы размеры контрола изменялись вместе с размером формы.
            gMapControl1.MapProvider = GMapProviders.GoogleMap; //Указываем что будут использоваться карты OpenStreetMaps. Здесь куча карт на выбор.
            GMaps.Instance.Mode = AccessMode.ServerOnly;

            gMapControl1.Position = new PointLatLng(40, -100);
        }
        private void GetData(object sender, EventArgs e)
        {

            states = service.GetStates();
            keys = new List<string>(states.Keys);
            tweets = service.GetTweets();
            states = service.GroupTweetsByState(tweets);
            tweets = service.GetTweetsSentiments(tweets);
            statesSentiments = service.GetStatesSentiments(states);
            colors = service.GetColors(statesSentiments);
        }
        private void DrawPolygons(object sender, EventArgs e)
        { 
            List<PointLatLng> points = new List<PointLatLng>();
            for (int i = 0; i < states.Values.Count; i++)
            {
                for (int j = 0; j < states[keys[i]].Polygons.Count; j++)
                {
                    foreach (Polygon polygon in states[keys[i]].Polygons)
                    {
                        foreach (Business_Layer.Point point in polygon.Points)
                        {
                            points.Add(new PointLatLng(point.Coordinates[1], point.Coordinates[0]));
                        }
                        if (states[keys[i]].Tweets.Count.Equals(0))
                        {
                            DrawPolygon(points, keys[i], Color.Gray);
                        }
                        else
                        {
                            DrawPolygon(points, keys[i], colors[i]);
                        }
                        points.Clear();
                    }
                }
            }
            gMapControl1.Zoom += 0.5;
            gMapControl1.Zoom -= 0.5;
        }
        private void DrawMarkers(object sender, EventArgs e)
        {
            PointLatLng point;
            for (int i = 0; i < tweets.Count; i++)
            {
                if (tweets[i].Weight > 0)
                {
                    point = new PointLatLng(tweets[i].Point.Coordinates[1], tweets[i].Point.Coordinates[0]);
                    DrawMarker(point, tweets[i].Text, GMarkerGoogleType.green_small);
                }
                if (tweets[i].Weight < 0)
                {
                    point = new PointLatLng(tweets[i].Point.Coordinates[1], tweets[i].Point.Coordinates[0]);
                    DrawMarker(point, tweets[i].Text, GMarkerGoogleType.red_small);
                }
                if (tweets[i].Weight == 0)
                {
                    point = new PointLatLng(tweets[i].Point.Coordinates[1], tweets[i].Point.Coordinates[0]);
                    DrawMarker(point, tweets[i].Text, GMarkerGoogleType.white_small);
                }
            }
            gMapControl1.Zoom += 0.5;
            gMapControl1.Zoom -= 0.5;
        }
        private void DrawMarker(PointLatLng point, string text, GMarkerGoogleType gMarker)
        {
            GMapOverlay markersOverlay = new GMapOverlay("marker");
            GMapMarker marker = new GMarkerGoogle(point, gMarker);
            marker.ToolTip = new GMapRoundedToolTip(marker);
            marker.ToolTipText = text;
            markersOverlay.Markers.Add(marker);
            gMapControl1.Overlays.Add(markersOverlay);

        }
        private void DrawPolygon(List<PointLatLng> points, string code, Color color)
        {
            GMapOverlay overlay = new GMapOverlay("polygon");
            GMapPolygon mapPolygon = new GMapPolygon(points, code);
            mapPolygon.Tag = code;
            mapPolygon.Fill = new SolidBrush(color);
            mapPolygon.Stroke = new Pen(Color.Red, 1);
            overlay.Polygons.Add(mapPolygon);
            gMapControl1.Overlays.Add(overlay);
        }

        private void FilesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            gMapControl1.Overlays.Clear();
            gMapControl1.Refresh();
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            service.ParseTweets(filesComboBox.SelectedItem.ToString());
        }

        private void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                gMapControl1.MarkersEnabled = true;
            }
            else
            {
                gMapControl1.MarkersEnabled = false;
            }
        }
    }
}
