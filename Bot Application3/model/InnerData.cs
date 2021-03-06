﻿using System.Collections.Generic;

namespace Bot.model
{
    public class InnerData
    {
        public static Dictionary<string, string> dic = null;
        public static Dictionary<string, string> questionAnswer = null;
        static InnerData()
        {
            dic = new Dictionary<string, string>();
            questionAnswer = new Dictionary<string, string>();
            questionAnswer.Add("客户投诉本人", "# 抱歉给您带来不好的体验，马上将您的问题提交我的上级领导，他会1小时内与您联系，希望您能谅解！\n\n"
                           );
            questionAnswer.Add("客户投诉其他部门", "# 抱歉给您带来不好的体验，请您详细说下当时的情况，我们会马上向相关部门反映情况。\n\n"
                           );
            questionAnswer.Add("投诉产品质量", "# 抱歉给您带来不好的体验，请您详细说下您遇到的问题，我们会马上向产品部门反映情况。您方便留下联系方式么？以便于我们后续联系您。\n\n"
                           );
            dic.Add("0", "# 欢迎使用！\n\n" +
                      "1. 屏幕问题\n\n" +
                      "2. 主机问题\n\n" +
                      "3. 电源问题\n\n" +
                      "9. 人工客服\n\n"
                         );
            dic.Add("1", "# 在刚开机时整个屏幕偏红（部分彩显会带有回扫线），但一眨眼之功夫就好了?\n\n" +
                                  "* 对于供电电阻呈断路损坏造成之故障现象和碰极一样为满屏带回扫线且某一色极亮，但其并不会导致保护性关机，解决方法很简单换同阻值之新电阻即可！当然，如果是阻值变大了，也要进行换新处理\n\n" +
                                  "# 屏幕上存在干扰故障\n\n" +
                                  "* 其真正之故障原因通常是显像管高压嘴和高压帽之间出现了打火现象。当然，也有很多检修人员能很快地分析这是由于高压嘴处打火造成之，但有很多人之处理方法都不太正确\n\n"
                            );
            dic.Add("2", "# 主机开关电源的市电插头松动，接触不良，没有插紧\n\n" +
                                  "* 更换优质的3C认证电源线\n\n" +
                                  "# 电脑打不出字\n\n" +
                                  "* 打开控制面板,单击“日期、时间、语言和区域设置”再单击“区域和语言选项”，在对话框的语言栏中单击“详细信息”，你会进入“文字服务和输入语言”对话框，在“高级”选项中，将系统配置中“关闭高级文字服务”前的勾去掉。再进入“设置”—“语言栏”，将“语言栏设置”选项中的第一第三项都选中（打勾），若需要，可增加一些必需的输入法，确定后退出\n\n"
                            );
            dic.Add("3", "# 电源指示灯亮但系统不运行，屏幕也没显示：\n\n" +
                                    "* 如果外接显示器能够正常显示，则通常可以认为处理器和内存等部件正常，故障部件可能为液晶屏、屏线、显卡和主板等。如果外接显示器也无法正常显示，则故障部件可能为显卡、主板、处理器和内存等。\n\n" +
                                    "# 开机或运行中死机、系统自动重启：\n\n" +
                                    "* 很多用户在使用笔记本时往往会遇到死机或者系统自动重启等问题，这一般来说是操作系统或者应用程序等软件问题——系统文件异常，或中病毒；机型不支持某操作系统、应用程序冲突导致系统死机\n\n"
                            );
        }
    }
}
