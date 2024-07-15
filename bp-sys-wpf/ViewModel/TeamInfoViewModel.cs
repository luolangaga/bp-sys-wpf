﻿using bp_sys_wpf.Model;
using bp_sys_wpf.Views.Pages;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using Wpf.Ui.Controls;
using System.IO;

namespace bp_sys_wpf.ViewModel
{
    public class TeamInfoViewModel : INotifyPropertyChanged
    {
        private int NowMainPlayerAccount = 0, NowAwayPlayerAccount = 0;
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
        private NowViewModel _nowView = new NowViewModel();

        public NowViewModel NowView
        {
            get { return _nowView; }
            set
            {
                _nowView = value;
                RaisePropertyChanged("NowView");
            }
        }

        private TeamInfoModel _TeamInfoModel;

        public TeamInfoModel TeamInfoModel//连接TeamInfoModel创建实例
        {
            get
            {
                if (_TeamInfoModel == null)//初始化
                {
                    _TeamInfoModel = new TeamInfoModel();
                    for (int i = 0; i < 9; i++)//选手列表阵营初始化
                    {
                        if (i < 6)
                        {
                            _TeamInfoModel.MainTeamPlayer.Add(new Player
                            {
                                Name = "",
                                State = "求生者"
                            });
                            _TeamInfoModel.AwayTeamPlayer.Add(new Player
                            {
                                Name = "",
                                State = "求生者"
                            });
                        }
                        else
                        {
                            _TeamInfoModel.MainTeamPlayer.Add(new Player
                            {
                                Name = "",
                                State = "监管者"
                            });
                            _TeamInfoModel.AwayTeamPlayer.Add(new Player
                            {
                                Name = "",
                                State = "监管者"
                            });
                        }
                    }
                    _TeamInfoModel.MainTeamInfo.State = "求生者";//队伍阵营初始化
                    _TeamInfoModel.AwayTeamInfo.State = "监管者";
                }
                if (_TeamInfoModel.MainTeamInfo.State == "求生者")
                {
                    NowView.NowModel.NowSurTeam.Name = _TeamInfoModel.MainTeamInfo.Name;
                    NowView.NowModel.NowHunTeam.Name = _TeamInfoModel.AwayTeamInfo.Name;
                    NowView.NowModel.NowSurTeam.LOGO = _TeamInfoModel.MainTeamInfo.LOGO;
                    NowView.NowModel.NowHunTeam.LOGO = _TeamInfoModel.AwayTeamInfo.LOGO;
                }
                else
                {
                    NowView.NowModel.NowHunTeam.Name = _TeamInfoModel.MainTeamInfo.Name;
                    NowView.NowModel.NowSurTeam.Name = _TeamInfoModel.AwayTeamInfo.Name;
                    NowView.NowModel.NowSurTeam.LOGO = _TeamInfoModel.AwayTeamInfo.LOGO;
                    NowView.NowModel.NowHunTeam.LOGO = _TeamInfoModel.MainTeamInfo.LOGO;
                }
                return _TeamInfoModel;
            }
            set
            {
                _TeamInfoModel = value;
                if (_TeamInfoModel.MainTeamInfo.State == "求生者")
                {
                    NowView.NowModel.NowSurTeam.Name = _TeamInfoModel.MainTeamInfo.Name;
                    NowView.NowModel.NowHunTeam.Name = _TeamInfoModel.AwayTeamInfo.Name;
                    NowView.NowModel.NowSurTeam.LOGO = _TeamInfoModel.MainTeamInfo.LOGO;
                    NowView.NowModel.NowHunTeam.LOGO = _TeamInfoModel.AwayTeamInfo.LOGO;
                }
                else
                {
                    NowView.NowModel.NowHunTeam.Name = _TeamInfoModel.MainTeamInfo.Name;
                    NowView.NowModel.NowSurTeam.Name = _TeamInfoModel.AwayTeamInfo.Name;
                    NowView.NowModel.NowSurTeam.LOGO = _TeamInfoModel.AwayTeamInfo.LOGO;
                    NowView.NowModel.NowHunTeam.LOGO = _TeamInfoModel.MainTeamInfo.LOGO;
                }
                RaisePropertyChanged("TeamInfoModel");
            }
        }

        private TeamInfoPageButtonModel _ButtonState;

        public TeamInfoPageButtonModel ButtonState
        {
            get
            {
                if (_ButtonState == null)
                {
                    _ButtonState = new TeamInfoPageButtonModel();
                    for (int i = 0; i < 9; i++)
                    {
                        _ButtonState.MainButtonState.Add(new TeamInfoPageButton());
                        _ButtonState.AwayButtonState.Add(new TeamInfoPageButton());
                    }
                }
                return _ButtonState;
            }
            set
            {
                _ButtonState = value;
                RaisePropertyChanged("ButtonState");
            }
        }
        public string OpenImageFileDialog()//打开通用对话框选取图片
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "图片文件|*.png;*.jpg"; // 设置过滤器只显示 PNG 和 JPG 文件  
            openFileDialog.Multiselect = false; // 设置只能选择一个文件  

            if (openFileDialog.ShowDialog() == true)
            {
                string FilePath = openFileDialog.FileName; // 获取选定的文件路径
                Debug.WriteLine(FilePath);
                return FilePath;
            }
            return "0";
        }
        public void SetTeamLOGO(string type)
        {
            string logo = OpenImageFileDialog();
            if (logo != "0")
            {
                if (type == "main")
                {
                    TeamInfoModel.MainTeamInfo.LOGO = new BitmapImage(new Uri(logo));
                }
                if (type == "away")
                {
                    TeamInfoModel.AwayTeamInfo.LOGO = new BitmapImage(new Uri(logo));
                }
                TeamInfoModel = TeamInfoModel;
            }
        }
        public void PlayersTakeTheField(int number, string team, string type)//选手上场
        {
            if (type == "求生者")//传入的上场选手为求生
            {
                if (team == "Main")
                {
                    //切换上场状态
                    TeamInfoModel.MainTeamPlayer[number].IsPlayerTakeTheField = true;
                    //按钮样式更改
                    ButtonState.MainButtonState[number].Content = "下场";
                    ButtonState.MainButtonState[number].Icon = new Wpf.Ui.Controls.SymbolIcon { Symbol = Wpf.Ui.Controls.SymbolRegular.ArrowDownload24 };
                    ButtonState.MainButtonState[number].Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7FFF0000"));
                    //增加上场选手计数器
                    NowMainPlayerAccount++;
                    //同步到当前上场列表
                    for (int i = 0; i < 5; i++)
                    {
                        if (TeamInfoModel.MainTeamInfo.State == "求生者" && NowView.NowModel.NowPlayer[i] == "")
                        {
                            NowView.NowModel.NowPlayer[i] = TeamInfoModel.MainTeamPlayer[number].Name;
                            break;
                        }
                    }

                }
                if (team == "Away")
                {
                    //切换上场状态
                    TeamInfoModel.AwayTeamPlayer[number].IsPlayerTakeTheField = true;
                    //按钮样式更改
                    ButtonState.AwayButtonState[number].Content = "下场";
                    ButtonState.AwayButtonState[number].Icon = new Wpf.Ui.Controls.SymbolIcon { Symbol = Wpf.Ui.Controls.SymbolRegular.ArrowDownload24 };
                    ButtonState.AwayButtonState[number].Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7FFF0000"));
                    //增加上场选手计数器
                    NowAwayPlayerAccount++;
                    for (int i = 0; i < 5; i++)
                    {
                        //同步到当前上场列表
                        if (TeamInfoModel.AwayTeamInfo.State == "求生者" && NowView.NowModel.NowPlayer[i] == "")
                        {
                            NowView.NowModel.NowPlayer[i] = TeamInfoModel.AwayTeamPlayer[number].Name;
                            break;
                        }
                    }
                }
                //判断满员，禁用上场按钮
                if (NowMainPlayerAccount >= 4)//主队满员
                {
                    for (int j = 0; j < 6; j++)
                    {
                        if (ButtonState.MainButtonState[j].Content == "上场")
                        {
                            ButtonState.MainButtonState[j].IsEnabled = false;//禁用按钮
                        }
                    }
                }
                if (NowAwayPlayerAccount >= 4)//客队满员
                {
                    for (int j = 0; j < 6; j++)
                    {
                        if (ButtonState.AwayButtonState[j].Content == "上场")
                        {
                            ButtonState.AwayButtonState[j].IsEnabled = false;//禁用按钮
                        }
                    }
                }
            }
            else//传入的上场选手为监管
            {
                if (team == "Main")
                {
                    //切换上场状态
                    TeamInfoModel.MainTeamPlayer[number].IsPlayerTakeTheField = true;
                    //按钮样式更改
                    ButtonState.MainButtonState[number].Content = "下场";
                    ButtonState.MainButtonState[number].Icon = new Wpf.Ui.Controls.SymbolIcon { Symbol = Wpf.Ui.Controls.SymbolRegular.ArrowDownload24 };
                    ButtonState.MainButtonState[number].Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7FFF0000"));
                    //同步到当前上场列表
                    if (TeamInfoModel.MainTeamInfo.State == "监管者" && NowView.NowModel.NowPlayer[4] == "")
                        NowView.NowModel.NowPlayer[4] = TeamInfoModel.MainTeamPlayer[number].Name;
                    //满员禁用上场按钮
                    for (int j = 6; j < 9; j++)
                    {
                        if (ButtonState.MainButtonState[j].Content == "上场")
                            ButtonState.MainButtonState[j].IsEnabled = false;
                    }
                }
                if (team == "Away")
                {
                    //切换上场状态
                    TeamInfoModel.AwayTeamPlayer[number].IsPlayerTakeTheField = true;
                    //按钮样式更改
                    ButtonState.AwayButtonState[number].Content = "下场";
                    ButtonState.AwayButtonState[number].Icon = new Wpf.Ui.Controls.SymbolIcon { Symbol = Wpf.Ui.Controls.SymbolRegular.ArrowDownload24 };
                    ButtonState.AwayButtonState[number].Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7FFF0000"));
                    //同步到当前上场列表
                    if (TeamInfoModel.AwayTeamInfo.State == "监管者")
                        NowView.NowModel.NowPlayer[4] = TeamInfoModel.AwayTeamPlayer[number].Name;
                    //满员禁用上场按钮
                    for (int j = 6; j < 9; j++)
                    {
                        if (ButtonState.AwayButtonState[j].Content == "上场")
                            ButtonState.AwayButtonState[j].IsEnabled = false;
                    }
                }
            }
            TeamInfoModel = TeamInfoModel;
            ButtonState = ButtonState;
            NowView = NowView;
        }
        public void PlayerOff(int number, string team, string type)//选手下场
        {

            if (type == "求生者")//传入的上场选手为求生
            {

                if (team == "Main")
                {
                    //切换上场状态
                    TeamInfoModel.MainTeamPlayer[number].IsPlayerTakeTheField = false;
                    //按钮样式更改
                    ButtonState.MainButtonState[number].Content = "上场";
                    ButtonState.MainButtonState[number].Icon = new Wpf.Ui.Controls.SymbolIcon { Symbol = Wpf.Ui.Controls.SymbolRegular.ArrowExportUp24 };
                    ButtonState.MainButtonState[number].Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7F00FF00"));
                    //减少上场选手计数器
                    NowMainPlayerAccount--;
                    for (int i = 0; i < 5; i++)
                    {
                        //同步到当前上场列表
                        if (TeamInfoModel.MainTeamInfo.State == "求生者" && NowView.NowModel.NowPlayer[i] == TeamInfoModel.MainTeamPlayer[number].Name)
                        {
                            NowView.NowModel.NowPlayer[i] = "";
                            break;
                        }
                    }
                    for (int j = 0; j < 6; j++)//主队解除上场限制
                    {
                        ButtonState.MainButtonState[j].IsEnabled = true;
                    }
                }
                if (team == "Away")
                {
                    //切换上场状态
                    TeamInfoModel.AwayTeamPlayer[number].IsPlayerTakeTheField = false;
                    //按钮样式更改
                    ButtonState.AwayButtonState[number].Content = "上场";
                    ButtonState.AwayButtonState[number].Icon = new Wpf.Ui.Controls.SymbolIcon { Symbol = Wpf.Ui.Controls.SymbolRegular.ArrowExportUp24 };
                    ButtonState.AwayButtonState[number].Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7F00FF00"));
                    //减少上场选手计数器
                    NowAwayPlayerAccount--;
                    for (int i = 0; i < 5; i++)
                    {
                        //同步到当前上场列表
                        if (TeamInfoModel.AwayTeamInfo.State == "求生者" && NowView.NowModel.NowPlayer[i] == TeamInfoModel.AwayTeamPlayer[number].Name)
                        {
                            NowView.NowModel.NowPlayer[i] = "";
                            break;
                        }
                    }
                    for (int j = 0; j < 6; j++)//客队解除上场限制
                    {
                        ButtonState.AwayButtonState[j].IsEnabled = true;
                    }

                }
            }
            else//传入的上场选手为监管
            {
                if (team == "Main")
                {
                    //切换上场状态
                    TeamInfoModel.MainTeamPlayer[number].IsPlayerTakeTheField = false;
                    //按钮样式更改
                    ButtonState.MainButtonState[number].Content = "上场";
                    ButtonState.MainButtonState[number].Icon = new Wpf.Ui.Controls.SymbolIcon { Symbol = Wpf.Ui.Controls.SymbolRegular.ArrowExportUp24 };
                    ButtonState.MainButtonState[number].Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7F00FF00"));
                    //同步到当前上场列表
                    if (NowView.NowModel.NowPlayer[4] == TeamInfoModel.MainTeamPlayer[number].Name && TeamInfoModel.MainTeamInfo.State == "监管者")
                        NowView.NowModel.NowPlayer[4] = "";
                    //启用上场按钮
                    for (int j = 6; j < 9; j++)
                    {
                        ButtonState.MainButtonState[j].IsEnabled = true;
                    }
                }
                if (team == "Away")
                {
                    //切换上场状态
                    TeamInfoModel.AwayTeamPlayer[number].IsPlayerTakeTheField = false;
                    //按钮样式更改
                    ButtonState.AwayButtonState[number].Content = "上场";
                    ButtonState.AwayButtonState[number].Icon = new Wpf.Ui.Controls.SymbolIcon { Symbol = Wpf.Ui.Controls.SymbolRegular.ArrowExportUp24 };
                    ButtonState.AwayButtonState[number].Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7F00FF00"));
                    //同步到当前上场列表
                    if (NowView.NowModel.NowPlayer[4] == TeamInfoModel.AwayTeamPlayer[number].Name && TeamInfoModel.AwayTeamInfo.State == "监管者")
                        NowView.NowModel.NowPlayer[4] = "";
                    //启用上场按钮
                    for (int j = 6; j < 9; j++)
                    {
                        ButtonState.AwayButtonState[j].IsEnabled = true;
                    }
                }
            }
            TeamInfoModel = TeamInfoModel;
            ButtonState = ButtonState;
            NowView = NowView;
        }

        public void Swap()//换边
        {
            (TeamInfoModel.MainTeamInfo.State, TeamInfoModel.AwayTeamInfo.State) = (TeamInfoModel.AwayTeamInfo.State, TeamInfoModel.MainTeamInfo.State);
            //外层0~3代表当前上场选手 内层循环0~5 判断队伍是主队还是客队 选手名称判断内容：是否为上场选手、阵营，然后赋值给NowPlayer
            for (int i = 0; i < 5; i++)
            {
                NowView.NowModel.NowPlayer[i] = "";
            }
            for (int i = 0, j = 0; i < 6 && j < 4; i++)
            {
                if (TeamInfoModel.MainTeamInfo.State == "求生者")
                {
                    if (TeamInfoModel.MainTeamPlayer[i].State == "求生者" && TeamInfoModel.MainTeamPlayer[i].IsPlayerTakeTheField == true)
                    {
                        NowView.NowModel.NowPlayer[j] = TeamInfoModel.MainTeamPlayer[i].Name;
                        j++;
                    }
                }
                else
                {
                    if (TeamInfoModel.AwayTeamPlayer[i].State == "求生者" && TeamInfoModel.AwayTeamPlayer[i].IsPlayerTakeTheField == true)
                    {
                        NowView.NowModel.NowPlayer[j] = TeamInfoModel.AwayTeamPlayer[i].Name;
                        j++;
                    }
                }
            }
            //一层6~8循环 判断与上面相同 监管者
            for (int i = 6; i < 9; i++)
            {
                if (TeamInfoModel.MainTeamInfo.State == "监管者")
                {
                    if (TeamInfoModel.MainTeamPlayer[i].State == "监管者" && TeamInfoModel.MainTeamPlayer[i].IsPlayerTakeTheField == true)
                    {
                        NowView.NowModel.NowPlayer[4] = TeamInfoModel.MainTeamPlayer[i].Name;
                        break;
                    }
                }
                else
                {
                    if (TeamInfoModel.AwayTeamPlayer[i].State == "监管者" && TeamInfoModel.AwayTeamPlayer[i].IsPlayerTakeTheField == true)
                    {
                        NowView.NowModel.NowPlayer[4] = TeamInfoModel.AwayTeamPlayer[i].Name;
                        break;
                    }
                }
            }
            TeamInfoModel = TeamInfoModel;
            NowView = NowView;
        }
        public void SwapPlayers(int num1, int num2)
        {
            (NowView.NowModel.NowPlayer[num1], NowView.NowModel.NowPlayer[num2]) = (NowView.NowModel.NowPlayer[num2], NowView.NowModel.NowPlayer[num1]);
            TeamInfoModel = TeamInfoModel;
            NowView = NowView;
        }

        public void ImportTeamInfoFromJson(string team)
        {
            //调用通用对话框导入json文件
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Json files (*.json)|*.json";
            if (openFileDialog.ShowDialog() == true)
            {
                string json_path = openFileDialog.FileName;
                //读取json文件
                string json_str = File.ReadAllText(json_path);
                //解析json文件
                JObject json_obj = JObject.Parse(json_str);
                //获取队名
                string team_name = (string)json_obj["teamName"];
                //获取队标
                string logo_uri = (string)json_obj["LogoUri"];
                if (team == "main")
                {
                    TeamInfoModel.MainTeamInfo.Name = team_name;
                    TeamInfoModel.MainTeamInfo.LOGO = new BitmapImage(new Uri(logo_uri));
                }
                else
                {
                    TeamInfoModel.AwayTeamInfo.Name = team_name;
                    TeamInfoModel.AwayTeamInfo.LOGO = new BitmapImage(new Uri(logo_uri));
                }

                int SurProgress = 0, HunProgress = 6;
                // 遍历players数组  
                foreach (JObject player in json_obj["players"])
                {
                    string playerName = player["playerName"].ToString();
                    string type = player["type"].ToString();
                    // 根据type的值将选手名称添加到相应的列表中  
                    if (type == "sur")
                    {
                        if (team == "main")
                        {
                            TeamInfoModel.MainTeamPlayer[SurProgress].Name = playerName;
                        }
                        else
                        {
                            TeamInfoModel.AwayTeamPlayer[SurProgress].Name = playerName;
                        }
                        SurProgress++;
                    }
                    else if (type == "hun")
                    {
                        if (team == "main")
                        {
                            TeamInfoModel.MainTeamPlayer[HunProgress].Name = playerName;
                        }
                        else
                        {
                            TeamInfoModel.AwayTeamPlayer[HunProgress].Name = playerName;
                        }
                        HunProgress++;
                    }
                }
            }
            TeamInfoModel = TeamInfoModel;
            NowView = NowView;
        }
    }
}
