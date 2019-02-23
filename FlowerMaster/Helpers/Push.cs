﻿using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Drawing;
using mshtml;
using Nekoxy;
using MahApps.Metro.Controls.Dialogs;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows.Media;
using FlowerMaster;
using FlowerMaster.Models;
using FlowerMaster.Helpers;
using FlowerMaster.Properties;
using static FlowerMaster.CordCol;
using System.Threading;

namespace FlowerMaster.Helpers
{

    public class Nodes
    {
        //自动推兔2.0函数

        private IntPtr WebHandle = IntPtr.Zero;
        private int delay = 1000;
        private bool sblock = false;

        //核心功能


        private readonly Color Col = Color.Instance;
        private readonly Mouse Mou = Mouse.Instance;
        private readonly Counter PushTimes = Counter.Instance;

        public void ScInitialize(IntPtr Hand)
        {
            WebHandle = Hand;
            Col.Load(WebHandle);
            Mou.Load(WebHandle);
        }
        
        
        //Main
        /// <summary>
        /// 开始脚本与初始化数据
        /// </summary>
        public void Start()
        {
            Random rnd = new Random();
            while (PushTimes.Value() > 0 && 
                   DataUtil.Config.sysConfig.autoType == 0)
            {
                if (DataUtil.Config.sysConfig.actionPrep == true && 
                    Col.Check(540, 75, 55, 47, 44) == true)
                {
                    ScActionPrep();
                }

                //随即延迟
                delay = rnd.Next(DataUtil.Config.sysConfig.delayTime, DataUtil.Config.sysConfig.delayTime * 2);
                sblock = DataUtil.Config.sysConfig.specialBlock;

                while (ScSelect() == false) { Thread.Sleep(delay); }
                ScDepart();
                while (ScCombat() == false) { Thread.Sleep(delay); }

                CoHomeReturn();

                PushTimes.Decrease();

                if (Col.Check(625, 70, 243, 212, 0) == true &&
                    DataUtil.Config.sysConfig.raidOther == true)
                {
                    CoHomeReturn();
                    ScGranRaid();
                }

                if (DataUtil.Config.sysConfig.sellTrue == true)
                {
                    CoHomeReturn();
                    ScSell(); 
                }

                if (DataUtil.Config.sysConfig.exploreTrue == true)
                {
                    CoHomeReturn();
                    WaMainLoad();
                    if (Col.Check(258, 163, 99, 99, 99) == false &&
                       (Col.Check(520, 75, 50, 41, 37) == true ||
                        Col.Check(520, 77, 83, 81, 76) == true))
                    {
                        ScExplore();
                    } 
                }

                if (DataUtil.Config.sysConfig.gardenTrue == true)
                {
                    CoHomeReturn();
                    if (DataUtil.Game.player.plantTime < DataUtil.Game.serverTime)
                    {
                        ScGarden();
                    } 
                }

            }

            while (PushTimes.Value() > 0 &&
                   DataUtil.Config.sysConfig.autoType == 1)
            {

                //随机延迟
                delay = rnd.Next(DataUtil.Config.sysConfig.delayTime, DataUtil.Config.sysConfig.delayTime * 2);
                Thread.Sleep(delay);

                if (Col.Check(625, 70, 243, 212, 0) == true &&
                    DataUtil.Config.sysConfig.raidOther == true)
                {
                    CoHomeReturn();
                    ScGranRaid();
                }

                if (DataUtil.Config.sysConfig.exploreTrue == true)
                {
                    CoHomeReturn();
                    if (Col.Check(258, 163, 99, 99, 99) == false &&
                       (Col.Check(520, 75, 50, 41, 37) == true ||
                        Col.Check(520, 77, 83, 81, 76) == true))
                    {
                        ScExplore();
                    }
                }

                if (DataUtil.Config.sysConfig.gardenTrue == true)
                {
                    CoHomeReturn();
                    if (DataUtil.Game.player.plantTime < DataUtil.Game.serverTime)
                    {
                        ScGarden();
                    }
                }

                CoHomeReturn();
                Thread.Sleep(delay);
            }

            return;
        }

        //脚本事件 - Sc系列
        /// <summary>
        /// 选择关卡，并恢复体力
        /// </summary>
        /// <param name="Node"></param>
        private bool ScSelect()
        {
            CoHomeDepart();

            //等待出击页面加载结束
            while (Col.Check(220, 300, 249, 248, 240) == false) { Thread.Sleep(delay); }


            //根据选择点击出击页面

            //进入主线推兔页面
            if (DataUtil.Config.sysConfig.pushType == 0)
            {
                while (Col.Check(300, 125, 207, 180, 122) == false)
                {
                    Mou.Click(300, 140);
                    Thread.Sleep(delay);
                }
                while (Col.Check(200, 225, 75, 70, 52) == false) { Thread.Sleep(delay); }
                Mou.Click(300, 260);
                while (Col.Check(218, 241, 241, 237, 222) == false) { Thread.Sleep(delay); }
                Mou.Click(300, 260);
            }

            //进入活动推兔页面
            if (DataUtil.Config.sysConfig.pushType == 1)
            {
                if (sblock == false)
                {
                    while (Col.Check(218, 325, 241, 237, 222) == false)
                    {
                        Mou.Click(400, 140);
                        Thread.Sleep(delay);
                    }
                    Mou.Click(300, 330);
                }
                //适配额外的格子
                else
                {
                    while (Col.Check(218, 409, 241, 237, 222) == false)
                    {
                        Mou.Click(400, 140);
                        Thread.Sleep(delay);
                    }
                    Mou.Click(300, 414);
                }
            }

            //进入活动特命-投票道具页面
            if (DataUtil.Config.sysConfig.pushType == 3)
            {
                //重新回到主页
                CoHomeReturn();
                //在进入活动页面之前 - 这里取要是背景变成了默认背景那就进入了 - 不停的点活动所在处的地址
                while (Col.Check(140, 615, 84, 126, 21) == false)
                {
                    Mou.Click(375, 405);
                    Thread.Sleep(delay);
                }
                //这里选择要推的...四个if...
                while (Col.Check(805, 210, 111, 115, 91) == false)
                {
                    if (DataUtil.Config.sysConfig.specTarget == 0) Mou.Click(570, 415);
                    if (DataUtil.Config.sysConfig.specTarget == 1) Mou.Click(810, 415);
                    if (DataUtil.Config.sysConfig.specTarget == 2) Mou.Click(570, 550);
                    if (DataUtil.Config.sysConfig.specTarget == 3) Mou.Click(810, 550);
                    Thread.Sleep(delay);
                }
                //开启特命 - 如果推图次数>=5000则一次一次推,如果小于5000则最大化再推
                if (PushTimes.Value() < 5000)
                { Mou.Click(600, 320); }
                Thread.Sleep(delay);
                Mou.Click(410, 465);

                //等待特命出现
                while (Col.Check(805, 210, 180, 184, 146) == false)
                {
                    //无聊点击加快特命出现时间
                    Mou.Click(805, 210);
                    Thread.Sleep(delay);
                }

                //从进入特命图那搬来的代码...不知道会不会崩掉
                //确认开始特命副本
                while (Col.Check(445, 406, 61, 20, 13) == true)
                { Mou.Click(400, 400); }
                while (Col.Check(280, 210, 143, 118, 93) == false)
                { Thread.Sleep(delay); }


                //根据可以进入的图点击进图
                for (int i = 0; i < 6; i++)
                { 
                    if (Col.Check(218, 282 + 84 * i, 248, 246, 239) == true)
                    { Mou.Click(218, 282 + 84 * i); }
                    i++;
                }

            }

            ////进入水影推兔页面
            //if (DataUtil.Config.sysConfig.pushType == 2)
            //{
            //    while (Col.Check(620, 275, 249, 247, 240) == false)
            //    {
            //        Mou.Click(590, 140);
            //        Thread.Sleep(delay);
            //    }
            //    while (Col.Check(620, 275, 249, 247, 240) == true)
            //    {
            //        Mou.Click(600, 250);
            //        Thread.Sleep(delay);
            //    }
            //}
            
            //尝试进入队友选择
            if (DataUtil.Config.sysConfig.pushType != 2)
            {
                CoDepartFirst();
                //确认体力页面是否或者推兔页面是否出现
                return ScStageDecision();
            }

            //进入上一次推的图
            if (DataUtil.Config.sysConfig.pushType == 2)
            {
                CoDepartPrevious();
                //确认体力页面是否或者推兔页面是否出现
                while (true)
                {
                    //判断体力恢复是否出现
                    if (Col.Check(320, 320, 176, 31, 69) == true &&
                        Col.Check(320, 225, 209, 192, 82) == true)
                    {
                        //如果碎石失败，返回主页面
                        if (ScRefill() == false) return false;
                        WaMainLoad();
                        CoDepartPrevious();
                        return true;
                    }

                    //判断队友选择是否出现
                    if (Col.Check(934, 200, 55, 46, 5) == true ||
                        Col.Check(922, 229, 205, 167, 111) == true)
                    {
                        return true;
                    }
                    Thread.Sleep(delay);
                }
            }
            return true;
        }

        /// <summary>
        /// 判定 【点击第一个图】 后出现的是恢复体力还是队友选择窗口，并采取反应
        /// </summary>
        private bool ScStageDecision()
        {
            while (true)
            {
                //判断体力恢复是否出现
                if (Col.Check(320, 320, 176, 31, 69) == true &&
                    Col.Check(320, 225, 209, 192, 82) == true)
                {
                    //如果碎石失败，返回主页面
                    if (ScRefill() == false) return false;
                    WaMainLoad();
                    CoDepartFirst();
                    return true;
                }

                //判断队友选择是否出现
                if (Col.Check(934, 200, 55, 46, 5) == true ||
                    Col.Check(922, 229, 205, 167, 111) == true)
                {
                    return true;
                }
                Thread.Sleep(delay);
            }
        }

        /// <summary>
        /// 恢复体力脚本
        /// </summary>
        private bool ScRefill()
        {
            //如果不喝药+碎石，则退出碎石页面
            if (DataUtil.Config.sysConfig.potionTrue == false && DataUtil.Config.sysConfig.stoneTrue == false)
            {
                Mou.Click(500, 460);
                return false;
            }

            //喝药水
            if (DataUtil.Config.sysConfig.potionTrue == true)
            {
                //确认是否有药水喝
                if (Col.Check(300, 400, 128, 128, 128) == false)
                {
                    Mou.Click(300, 400);

                    WaConfirmWindow();
                    //确定喝药红字出现
                    if (Col.Check(341, 323, 255, 1, 1) == true)
                    {
                        Mou.Click(410, 400);
                        CoPrevent();
                        return true;
                    }
                }
                else if(DataUtil.Config.sysConfig.stoneTrue == false)
                {
                    Mou.Click(500, 460);
                    return false;
                }
            }

            //碎石头
            if (DataUtil.Config.sysConfig.stoneTrue == true)
            {
                while (Col.Check(410, 460, 201, 97, 82) == false)
                {
                    Mou.Click(650, 400);
                    Thread.Sleep(delay);
                }

                //确定碎石红字出现
                if (Col.Check(312, 400, 255, 1, 1) == true)
                {
                    Mou.Click(410, 460);
                    CoPrevent();
                    return true;
                }
                //红字没出现，没石头，点击退出
                else
                {
                    Mou.Click(500, 460);
                    return false;
                }
            }

            return false;
        }

        /// <summary>
        /// 开始推兔
        /// </summary>
        /// <param name="Node"></param>
        /// <returns></returns>
        private void ScDepart()
        {
            WaMainLoad();
            CoAssistSelect();
            CoMisssionLaunch();
            //等待成功进图
            while(Col.Check(900, 25, 1, 45, 44) == false) { Thread.Sleep(delay); }
            
            return;
        }

        /// <summary>
        /// 推兔过程中判定
        /// </summary>
        private bool ScCombat()
        {
            //如果出现弹窗，关闭并继续推兔
            if (Col.Check(795, 205, 6, 90, 89) == true)
            {
                Mou.Click(805, 205);
                return false;
            }

            //如果出现加战友，取消并继续推兔
            else if (Col.Check(640, 240, 84, 61, 43) == true)
            {
                Mou.Click(550, 430);
                return false;
            }

            //如果在主页，返还
            else if (Col.Check(170, 40, 163, 148, 66) == true &&
                     Col.Check(5, 5, 166, 161, 139) == true)
            {
                return true;
            }

            //如果出现Boss，根据选择启动函数
            else if (Col.Check(333, 410, 84, 26, 17) == true &&
                     Col.Check(710, 410, 68, 38, 1) == true)
            {
                if (DataUtil.Config.sysConfig.raidSelf == true)
                {
                    CoBossStart();
                    ScAttackRaid();
                    return true;
                }
                else
                {
                    ScPublicRaid();
                    return true;
                }
            }

            //如果出现特命，根据选择启动函数
            else if (Col.Check(445, 406, 61, 20, 13) == true &&
                     Col.Check(590, 406, 1, 45, 45) == true)
            {
                ScSpecial();
                return true;
            }

            //如果出现眷属的自动Boss弹窗，关闭并判定推图结束
            else if (Col.Check(340, 390, 253, 156, 142) == true &&
                     Col.Check(460, 390, 253, 155, 141) == true &&
                     Col.Check(510, 390, 101, 218, 215) == true)
            {
                Mou.Click(550, 400);
                return true;
            }
            
            //如果无事件，继续推兔
            else
            {
                Mou.Click(830, 550);
                Thread.Sleep(delay);
                return false;
            }

        }
        
        /// <summary>
        /// 点击Raid按钮，确定目前是眷属战或者普通Raid，并根据结果继续
        /// </summary>
        private void ScGranRaid()
        {
            while (Col.Check(500, 300, 119, 82, 69) == false &&
                   Col.Check(200, 170, 212, 184, 131) == false)
            {
                Mou.Click(355, 160);
                Thread.Sleep(delay);
            }

            if (Col.Check(500, 300, 119, 82, 69) == true)
            {
                ScGranBoss();
            }
            else ScAttackRaid();
        }

        /// <summary>
        /// 进入主页Boss战并放野
        /// </summary>
        /// <param name="Node"></param>
        private void ScAttackRaid()
        {
            //如果没Boss，退出Boss页面
            while (Col.Check(200, 170, 212, 184, 131) == false) { Thread.Sleep(delay); }
            while (Col.Check(340, 260, 207, 170, 110) == true)
            {
                Thread.Sleep(delay);
                WaMainLoad();

                //循环确认三个Boss，如果没有还没打的Boss的话，回主页
                for (int i = 0; i < 3; i++)
                {
                    if ((Col.Check(870, 260 + 148*i, 80, 26, 17) == false &&
                         Col.Check(510, 122 + 148*i, 249, 248, 240) == false))
                    {
                        CoHomeReturn();
                        return;
                    }
                }
            }

            CoBossEnter();
            CoMisssionLaunch();
            while (Col.Check(550, 600, 227, 210, 175) == false) { Thread.Sleep(delay); }
            CoBossAttack();

            //判定是否出现无Boss点碎石页面
            while (true)
            {
                //无Boss点 要求碎石
                if (Col.Check(400, 455, 231, 129, 114) == true)
                {
                    Thread.Sleep(100);
                    //取消碎石
                    Mou.Click(550, 460);
                    while (Col.Check(255, 135, 43, 24, 0) == false) { Thread.Sleep(delay); }
                    //请求援助
                    Mou.Click(220, 130);
                    CoBossAssist();
                    //等待并取消弹窗
                    CoPrevent();

                    return;
                }
                //有Boss点 成功进入
                if (Col.Check(630, 540, 0, 0, 0) == true) { break; }
                Thread.Sleep(100);
            }

            //等到能快时快进
            while (Col.Check(630, 540, 0, 0, 0) == true) { Thread.Sleep(delay); }
            CoBossSkip();

            //判定请求支援是否出现并采取措施
            while (true) 
            {
                if (Col.Check(290, 400, 175, 74, 59) == true &&
                    Col.Check(300, 400, 186, 84, 68) == true)
                {
                    CoBossAssist();
                    CoPrevent();
                    return;
                }
                if (Col.Check(5, 634, 71, 61, 21) == true)
                {
                    return;
                }
            }
        }

        /// <summary>
        /// 直接放野主页Boss
        /// </summary>
        /// <param name="Node"></param>
        private void ScPublicRaid()
        {
            CoBossPublic();
            CoBossAssist();
            //等待并取消弹窗
            CoPrevent();

            return;
        }

        /// <summary>
        /// 眷属战Boss时特用脚本，使用全部Boss点攻击眷属Boss
        /// </summary>
        private void ScGranBoss()
        {
            CoGranEnter();
            CoMisssionLaunch();
            CoGranStart();

            //眷属战结束前不停的点击跳过
            while (Col.Check(780, 85, 2, 85, 86) == false)
            {
                Mou.Click(910, 430);
                Thread.Sleep(delay);
            }

            Mou.Click(780, 85);

            return;
        }

        /// <summary>
        /// 特命脚本 包含进战过程
        /// </summary>
        private void ScSpecial()
        {
            //判定是否进入特命]
            if (DataUtil.Config.sysConfig.specialTrue == true)
            {
                //确认开始特命副本
                while (Col.Check(445, 406, 61, 20, 13) == true)
                {
                    Mou.Click(400, 400);
                }
                while (Col.Check(280, 210, 143, 118, 93) == false)
                { Thread.Sleep(delay); }


                //根据可以进入的图点击进图
                bool Found = false;
                int i = 0;
                while (Found == false)
                {
                    if (i == 6)
                    {
                        CoSpecialExit();
                        return;
                    }
                    if (Col.Check(218, 282 + 84 * i, 248, 246, 239) == true)
                    {
                        Mou.Click(218, 282 + 84 * i);
                        Found = true;
                    }
                    i++;
                }

                //if(Col.Check(218, 241, 241, 237, 222) == true)
                //{
                //    Mou.Click(250, 250);
                //}

                //if (Col.Check(218, 325, 241, 237, 222) == true)
                //{
                //    Mou.Click(250, 350);
                //}
                
                CoDepartFirst();
                while (true)
                {
                    Thread.Sleep(delay);
                    //判断体力恢复是否出现
                    if (Col.Check(320, 320, 176, 31, 69) == true &&
                        Col.Check(320, 225, 209, 192, 82) == true)
                    {
                        //如果碎石失败，返回主页面
                        if (ScRefill() == false) return;

                        CoDepartFirst();
                        break;
                    }

                    //判断队友选择是否出现
                    if (Col.Check(934, 200, 55, 46, 5) == true ||
                        Col.Check(922, 229, 205, 167, 111) == true)
                    {
                        break;
                    }
                }
                ScDepart();
                while (ScCombat() == false) { Thread.Sleep(delay); }
                return;
            }
            else
            {
                CoSpecialExit();
            }

            return;
        }

        /// <summary>
        /// 出售花
        /// </summary>
        /// <param name="Node"></param>
        private void ScSell()
        {
            CoHomeTeam();
            CoTeamSell();
            while (Col.Check(325, 300, 143, 115, 86) == true) { Thread.Sleep(delay); }
            WaMainLoad();
            CoSellAll();

            //判定是否有花可卖
            while (true)
            {
                while (Col.Check(150, 620, 139, 42, 37) == false) { Thread.Sleep(delay); }
                //没花 点击取消
                if (Col.Check(420, 560, 51, 51, 51) == true)
                {
                    Mou.Click(810, 65);
                    return;
                }
                //有花 点击确认
                else if (Col.Check(390, 565, 63, 20, 13) == true)
                {
                        CoSellConfirm();
                        Thread.Sleep(delay); 
                    return;
                }
                else
                {
                    Thread.Sleep(delay);
                }
            }
        }

        /// <summary>
        /// 确认是否有探索点并探索一次
        /// </summary>
        /// <param name="Node"></param>
        /// <returns></returns>
        private void ScExplore()
        {
            //开始探索
            Mou.Click(275, 150);
            while (Col.Check(170, 40, 49, 45, 20) == false &&
                   Col.Check(170, 40, 116, 104, 46) == false) { Thread.Sleep(delay); }
            //等到探索页面出现后开始连点
            while (Col.Check(170, 40, 163, 148, 66) == false)
            {
                Mou.Click(800, 200);
                Thread.Sleep(100);
            }
            return;
        }

        /// <summary>
        /// 确认是否有花园虫并捕获
        /// </summary>
        /// <param name="Node"></param>
        /// <returns></returns>
        private void ScGarden()
        {
            //查看是否有花园虫，并收获
            Mou.Click(435, 150);
            while (Col.Check(380, 615, 34, 34, 34) == false &&
                   Col.Check(380, 615, 23, 23, 23) == false)
            {
                if (Col.Check(380, 615, 68, 0, 0) == true)
                {
                    Mou.Click(380, 615);
                }
                Thread.Sleep(delay);
            }

            //返回主页之前，关闭任何弹窗+点击返回主页
            while(Col.Check(170, 40, 163, 148, 66) == false)
            {
                if (Col.Check(850, 150, 1, 75, 73) == true)
                {
                    Mou.Click(850, 150);
                }
                Mou.Click(80, 80);
                Thread.Sleep(delay / 2);
                Mou.Click(5, 5);
                Thread.Sleep(delay / 2);
            }
            return;
        }

        /// <summary>
        /// 提前恢复体力
        /// </summary>
        private void ScActionPrep()
        {
            WaMainLoad();
            Mou.Click(80, 360);
            while (Col.Check(300, 380, 202, 165, 144) == false) { Thread.Sleep(delay); }
            Mou.Click(300, 380);
            while (Col.Check(320, 320, 176, 31, 69) == false) { Thread.Sleep(delay); }
            ScRefill();

        }

        /* Place Holder
         */


        //鼠标验色与点击事件 - Co系列

        /// <summary>
        /// 等到弹窗出现之后，在回到主页面或画面恢复正常之前并且弹窗消失前，不停等待并关闭弹窗
        /// </summary>
        private void CoPrevent()
        {
            while (Col.Check(795, 205, 6, 90, 89) == false) { Thread.Sleep(delay); }
            while (Col.Check(5, 634, 71, 61, 21) == false &&
                   Col.Check(795, 205, 6, 90, 89) == true)
            {
                Mou.Click(805, 205);
                Thread.Sleep(delay);
            }

            WaMainLoad();
        }

        /// <summary>
        /// 等待下方黑条载入结束后 确认在主页并点击出击按钮
        /// </summary>
        /// <returns></returns>
        private void CoHomeDepart()
        {
            while (Col.Check(400, 235, 171, 102, 43) == false)
            {
                if (Col.Check(400, 235, 0, 0, 0) == false)
                { Mou.Click(80, 155); }
                Thread.Sleep(delay);
            }
        }

        /// <summary>
        /// 等待下方黑条载入结束后 确认在主页并点击编队按钮
        /// </summary>
        /// <returns></returns>
        private void CoHomeTeam()
        {
            WaMainLoad();
            while (Col.Check(200, 225, 167, 211, 226) == false)
            {
                if (Col.Check(200, 225, 167, 211, 226) == false &&
                    Col.Check(200, 225, 0, 0, 0) == false)
                {
                    Mou.Click(80, 205);
                }
                Thread.Sleep(delay);
            }
        }

        /// <summary>
        /// 等待第一个可推兔出现并点击
        /// </summary>
        /// <returns></returns>
        private void CoDepartFirst()
        {
            while (Col.Check(600, 265, 28, 29, 19) == false) { Thread.Sleep(delay); }
            if (Col.Check(934, 200, 55, 46, 5) == false &&
                Col.Check(922, 229, 205, 167, 111) == false &&
                Col.Check(320, 320, 176, 31, 69) == false)
            {
                Mou.Click(430, 245);
                Thread.Sleep(delay);
            }
            Thread.Sleep(delay);
        }

        /// <summary>
        /// 确认上一次图可点，在进入选队友、出击、或者体力恢复之前不停点上一次图
        /// 三个可接受的颜色/坐标组合：正常图，正常图变色，眷属图
        /// </summary>
        private void CoDepartPrevious()
        {
            if (sblock == false)
            {
                while (Col.Check(600, 395, 97, 79, 32) == false &&
                       Col.Check(600, 395, 95, 79, 40) == false &&
                       Col.Check(600, 395, 89, 72, 28) == false &&
                       Col.Check(600, 375, 97, 79, 32) == false &&
                       Col.Check(600, 375, 95, 79, 40) == false &&
                       Col.Check(600, 375, 89, 72, 28) == false)
                { Thread.Sleep(delay); }

                while (Col.Check(934, 200, 55, 46, 5) == false &&
                    Col.Check(922, 229, 205, 167, 111) == false &&
                    Col.Check(320, 320, 176, 31, 69) == false)
                {
                    Mou.Click(250, 400);
                    Thread.Sleep(delay);
                }
            }
            //适配额外的格子
            else
            {
                while (Col.Check(600, 459, 156, 139, 126) == false)
                { Thread.Sleep(delay); }

                while (Col.Check(934, 200, 55, 46, 5) == false &&
                    Col.Check(922, 229, 205, 167, 111) == false &&
                    Col.Check(320, 320, 176, 31, 69) == false)
                {
                    Mou.Click(250, 484);
                    Thread.Sleep(delay);
                }
            }
        }

        /// <summary>
        /// 战友选择成功之前不停地点击二号战友位
        /// </summary>
        /// <returns></returns>
        private void CoAssistSelect()
        {
            while (Col.Check(922, 229, 205, 167, 111) == false)
            {
                while (Col.Check(934, 200, 55, 46, 5) == false)
                {
                    Thread.Sleep(delay);
                    if (Col.Check(922, 229, 205, 167, 111) == true) break;
                }
                Mou.Click(600, 250);
                Thread.Sleep(delay);
            }
        }

        /// <summary>
        /// 在准备页面或者维护窗口出现时，不停的点击出击按钮并确定是否弹出维护窗
        /// </summary>
        /// <returns></returns>
        private void CoMisssionLaunch()
        {
            while (Col.Check(425, 200, 213, 185, 133) == true)
            {
                Mou.Click(830, 550);
                CoMaintainConfirm();
                Thread.Sleep(delay);
            }
        }

        /// <summary>
        /// 等待出现Boss并选择攻击Boss
        /// </summary>
        /// <returns></returns>
        private void CoBossStart()
        {
            while (Col.Check(333, 410, 84, 26, 17) == true)
            {
                Mou.Click(285, 400);
                Thread.Sleep(delay);
            }
        }

        /// <summary>
        /// 等待出现Boss并选择放野Boss
        /// </summary>
        /// <returns></returns>
        private void CoBossPublic()
        {
            while (Col.Check(333, 410, 84, 26, 17) == true)
            { 
                Mou.Click(650, 400);
                Thread.Sleep(delay);
            }
        }

        /// <summary>
        /// 等待Boss列表加载完毕并攻击第1/2/3个。
        /// 在进入准备页面之前不停地点击Boss的出击按钮。
        /// 每个Boss间距148
        /// </summary>
        /// <returns></returns>
        private void CoBossEnter()
        {
            //进入Boss页面之前开始等待
            while (Col.Check(870, 260, 80, 26, 17) == false) { Thread.Sleep(delay); }

            //确认第3个Boss没有在【参战中】
            if (Col.Check(510, 270, 249, 248, 240) == true)
            {
                //进入队友选择页面之前选择第1个Boss
                while (Col.Check(922, 229, 205, 167, 111) == false)
                {
                    Mou.Click(840, 250);
                    Thread.Sleep(delay);
                } 
            }

            //确认第2个Boss没有在【参战中】
            else if (Col.Check(510, 418, 249, 248, 240) == true)
            {
                //进入队友选择页面之前选择第2个Boss
                while (Col.Check(922, 229, 205, 167, 111) == false)
                {
                    Mou.Click(840, 398);
                    Thread.Sleep(delay);
                }
            }

            //前两个Boss都在参战中，直接进入第3个Boss
            else
            {
                //进入队友选择页面之前选择第3个Boss
                while (Col.Check(922, 229, 205, 167, 111) == false)
                {
                    Mou.Click(840, 546);
                    Thread.Sleep(delay);
                }
            }
        }

        /// <summary>
        /// 在进入下一步之前不停地选择普通攻击Boss。因延迟关系使用绝对值延迟100。
        /// 并同时确定是否弹出维护窗
        /// </summary>
        /// <returns></returns>
        private void CoBossAttack()
        {
            while (Col.Check(400, 455, 231, 129, 114) == false &&
                   Col.Check(630, 540, 0, 0, 0) == false)
            {
                Mou.Click(750, 555);
                CoMaintainConfirm();
                Thread.Sleep(100);
            }
        }

        /// <summary>
        /// 等待申请援助选项出现并点击
        /// </summary>
        /// <returns></returns>
        private void CoBossAssist()
        {
            while (Col.Check(290, 400, 175, 74, 59) == false ||
                   Col.Check(300, 400, 186, 84, 68) == false) { Thread.Sleep(delay); }
            while (Col.Check(290, 400, 175, 74, 59) == true)
            {
                Mou.Click(290, 400);
                Thread.Sleep(delay);
            }
        }

        /// <summary>
        /// 在眷属战页面，进入队伍确认之前不停地点击出击按钮
        /// </summary>
        private void CoGranEnter()
        {
            while (Col.Check(425, 200, 213, 185, 133) == false)
            {
                Mou.Click(750, 480);
                Thread.Sleep(delay);
            }
        }

        /// <summary>
        /// 等待弹窗出现，选择最大Boss点消耗并开始
        /// </summary>
        private void CoGranStart()
        {
            while (Col.Check(425, 200, 154, 32, 18) == false) { Thread.Sleep(delay); }
            while (Col.Check(590, 310, 138, 138, 138) == false)
            {
                Mou.Click(590, 310);
                Thread.Sleep(delay);
            }
            Mou.Click(400, 500);
            return;
        }

        /// <summary>
        /// 等待特命弹窗出现并选择取消
        /// </summary>
        /// <returns></returns>
        private void CoSpecialExit()
        {
            while (Col.Check(450, 400, 192, 89, 73) == false) { Thread.Sleep(delay); }
            Mou.Click(550, 400);
        }

        /// <summary>
        /// 确认进入编成页面并点击出售
        /// </summary>
        /// <returns></returns>
        private void CoTeamSell()
        {
            while (Col.Check(530, 130, 225, 190, 131) == false)
            {
                Thread.Sleep(delay);
                Mou.Click(535, 137);
            }
        }

        /// <summary>
        /// 确认进入出售页面并不停地点击出售
        /// 直到出售页面出现为止
        /// </summary>
        /// <returns></returns>
        private void CoSellAll()
        {
            while (Col.Check(780, 580, 237, 225, 198) == false)
            {
                Mou.Click(220, 295);
                Thread.Sleep(delay);
            }
        }

        /// <summary>
        /// 确认进入批量出售页面并点击出售
        /// </summary>
        /// <returns></returns>
        private void CoSellConfirm()
        {
            while (Col.Check(802, 530, 233, 216, 183) == false) { Thread.Sleep(delay); }
            Mou.Click(420, 560);
        }

        /// <summary>
        /// 连续点击Home与无效位置来返回主页
        /// </summary>
        /// <returns></returns>
        private void CoHomeReturn()
        {
            while (Col.Check(437, 177, 211, 209, 205) == false)
            {
                Mou.Click(80, 80);
                Thread.Sleep(delay / 2);
                Mou.Click(5, 5);
                Thread.Sleep(delay / 2);
            }
        }

        /// <summary>
        /// 快进主页Boss
        /// </summary>
        /// <param name="Node"></param>
        /// <returns></returns>
        private void CoBossSkip()
        {
            while (Col.Check(290, 400, 175, 74, 59) == false &&
                   Col.Check(5, 634, 71, 61, 21) == false &&
                   Col.Check(290, 400, 175, 74, 59) == false)
            {
                Mou.Click(830, 550);
                Thread.Sleep(delay);
            }
        }

        /// <summary>
        /// 处理在每日维护之前的20分钟出击时出现的弹窗
        /// </summary>
        private void CoMaintainConfirm()
        {
            if(DataUtil.Game.serverTime.Hour == 3 &&
               DataUtil.Game.serverTime.Minute >= 40)
            {
                //延迟一秒等弹窗出现
                Thread.Sleep(1000);
                while (Col.Check(550, 330, 72, 55, 50) == true)
                {
                    Mou.Click(410, 400);
                    Thread.Sleep(delay);
                }
            }
        }


        //等待页面出现系列 - Wa

        /// <summary>
        /// 等待在主页时，下方黑条加载结束
        /// </summary>
        private void WaMainLoad()
        {
            while (Col.Check(5, 634, 71, 61, 21) == false) { Thread.Sleep(delay); }
        }

        /// <summary>
        /// 等待体力恢复确认框出现
        /// </summary>
        private void WaConfirmWindow()
        {
            while (Col.Check(410, 400, 190, 88, 73) == false) { Thread.Sleep(delay); }
        }
    }
}

