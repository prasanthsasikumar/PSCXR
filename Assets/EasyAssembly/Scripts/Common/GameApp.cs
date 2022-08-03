using System;
using System.Collections.Generic;

    public class GameApp{
         private static GameApp instance;
    public static GameApp Instance {
        get {
            if (instance == null)
                instance = new GameApp();
            return instance;
        }
    }


                   public CommonHintDlg MgrHintScript;

}
