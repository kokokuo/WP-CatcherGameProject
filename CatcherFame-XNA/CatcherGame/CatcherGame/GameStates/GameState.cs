﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//XNA Tool
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

using CatcherGame.GameObjects;
using CatcherGame.GameStates.Dialog;
using CatcherGame.TextureManager;
using CatcherGame.FontManager;

namespace CatcherGame.GameStates
{
    public abstract class GameState
    {
        protected SpriteBatch gameSateSpriteBatch;
        protected List<GameObject> gameObjects;
        protected int x;
        protected int y;
        protected int width;
        protected int height;
        protected bool isInit;
        protected Dictionary<DialogStateEnum, GameDialog> dialogTable;
        protected GameDialog pCurrentDialog;
        protected bool hasDialogShow;
        protected MainGame mainGame;
        protected Texture2D background; //此遊戲狀態的背景圖
        protected Vector2 backgroundPos;
        protected float leftGameScreenBorder,rightGameScreenBorder; //遊戲狀態的左右邊界
        protected int objIdCount;
        /// <summary>
        /// 取得背景圖
        /// </summary>
        /// <returns></returns>
        public Texture2D GetBackgroundTexture() {
            return this.background;
        }

        /// <summary>
        /// 取得目前遊戲中的Id值
        /// </summary>
        /// <returns></returns>
        public int GetObjId()
        {
            return this.objIdCount;
        }
        /// <summary>
        /// 累加ID
        /// </summary>
        public void AddObjId() {
            this.objIdCount++;
        }
        /// <summary>
        /// 取得遊戲螢幕的左邊邊框值
        /// </summary>
        /// <returns></returns>
        public float GetLeftGameScreenBorder() {
            return this.leftGameScreenBorder;
        }

        /// <summary>
        /// 取得遊戲螢幕的右邊邊框值
        /// </summary>
        /// <returns></returns>
        public float GetRightGameScreenBorder()
        {
            return this.rightGameScreenBorder;
        }

        public GameState(MainGame mainGamePointer)
        {
            gameObjects = new List<GameObject>();
            this.mainGame = mainGamePointer;
            
            isInit = false;
            width = mainGame.GetDeviceScreenWidth();
            height = mainGame.GetDeviceScreenHeight() ;
            hasDialogShow = false;
        }
        
        public abstract void LoadResource();
        public abstract void BeginInit();

        public bool IsEmptyQueue()
        {
            return mainGame.IsEmptyQueue();
            
        }
        /// <summary>
        /// 從MainGame中取得點擊時的資料
        /// </summary>
        /// <returns></returns>
        public TouchLocation GetTouchLocation()
        {
            return mainGame.GetTouchLocation();
        }

        /// <summary>
        /// 從MainGame中取得這次的所有點擊時的資料
        /// </summary>
        /// <returns></returns>
        public TouchCollection GetCurrentFrameTouchCollection()
        {
            return mainGame.GetCurrentFrameTouchCollection();
        }
        /// <summary>
        /// 如果有顯示對話框則更新對話框並停止遊戲物件更新,否則只會更新目前狀態中的遊戲物件
        /// </summary>
        public virtual void Update()
        {
            //如果有顯示對話框,則更新對話框的物件
            if (hasDialogShow)
            {
                //更新目前的對話框狀態
                pCurrentDialog.Update();
            }
            else {
                foreach (GameObject gameObject in gameObjects)
                {
                    gameObject.Update();
                }
            }
        }
        /// <summary>
        /// 繪製遊戲狀態中物件,但是如果有要顯示對話框,則也會一併繪製對話框
        /// </summary>
        public virtual void Draw()
        {
            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Draw(gameSateSpriteBatch);
            }
            //如果有要顯示對話框,繪製對話框
            if (hasDialogShow)
            {
                pCurrentDialog.Draw();
            }
        }
        /// <summary>
        /// 加入遊戲物件至此State
        /// </summary>
        /// <param name="gameObject"></param>
        public void AddGameObject(GameObject gameObject)
        {
            gameObjects.Add(gameObject);
        }


        /// <summary>
        /// 設定或取得遊戲狀態左上角x
        /// </summary>
        public int SetGetX
        {
            set { x = value; }
            get { return x; }
        }

        /// <summary>
        /// 設定或取得遊戲狀態的左上角y
        /// </summary>
        public int SetGetY
        {
            set { y = value; }
            get { return y; }
        }

        /// <summary>
        /// 設定或取得遊戲狀態的寬(以遊戲狀態為背景),預設為手機的螢幕寬
        /// </summary>
        public int SetGetWidth
        {
            set { width = value; }
            get { return width; }
        }

        /// <summary>
        /// 設定或取得遊戲狀態的高(以遊戲狀態為背景),預設為手機的螢幕高
        /// </summary>
        public int SetGetHeight
        {
            set { height = value; }
            get { return height; }
        }
        /// <summary>
        /// 取得此State所有遊戲物件
        /// </summary>
        public List<GameObject> GameObjects
        {
            get { return gameObjects; }
        }
        /// <summary>
        /// 取得遊戲狀態是否已經初始化(避免再次初始化,或是如果有需要可以把遊戲狀態釋放,重新設定無初始化)
        /// </summary>
        public bool GetGameStateHasInit {
            get { return isInit; }
        }


        /// <summary>
        /// 設定主遊戲中的SpriteBatch元件到 gameState 以協助繪製
        /// </summary>
        /// <param name="gSpriteBatch"></param>
        public void SetSpriteBatch(SpriteBatch gSpriteBatch)
        {
            this.gameSateSpriteBatch = gSpriteBatch;
        }
        

        /// <summary>
        /// 透過mainGame取得 已經載入好的 Texture2DList
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<Texture2D> GetTexture2DList(TexturesKeyEnum key)
        {
            return mainGame.GetTexture2DList(key);
        }

        
        /// <summary>
        /// 設定要談出哪個Dialog對話框視窗
        /// </summary>
        /// <param name="nextDialogKey">指定Enum的Key,如果要新增</param>
        public void SetPopGameDialog(DialogStateEnum nextDialogKey)
        {
            if (nextDialogKey != DialogStateEnum.EMPTY)
            {
                pCurrentDialog = dialogTable[nextDialogKey];
                hasDialogShow = true;
            }
            else
                hasDialogShow = false;
        }
        /// <summary>
        /// 取得時間戳記,給予動畫AnimationScripte使用
        /// </summary>
        /// <returns></returns>
        public TimeSpan GetTimeSpan()
        {
            return mainGame.TargetElapsedTime;
        }
        /// <summary>
        /// 取得SpriteBatch繪製圖像
        /// </summary>
        /// <returns></returns>
        public SpriteBatch GetSpriteBatch()
        {
            return gameSateSpriteBatch;
        }

        /// <summary>
        /// 設定切換至下一個的遊戲狀態
        /// </summary>
        /// <param name="key">下一個狀態的Enum key</param>
        public void SetNextGameSateByMain(GameStateEnum key) {
            this.mainGame.SetNextGameState(key);
        }


        /// <summary>
        /// 取得文字資源
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public SpriteFont GetSpriteFontFromKeyByGameState(SpriteFontKeyEnum key)
        {

            return mainGame.GetSpriteFontFromKeyByMainGame(key);
        }


        /// <summary>
        /// 透過mainGame清除TouchQueue裡面的所有狀態
        /// </summary>
        public void ClearTouchQueue()
        {
            mainGame.ClearTouchQueue();
        }


        public int GetDeviceScreenWidthByMainGame()
        {
            return mainGame.GetDeviceScreenWidth();
        }
        public int GetDeviceScreenHeightByMainGame()
        {
            return mainGame.GetDeviceScreenHeight();
        }
    }
}
