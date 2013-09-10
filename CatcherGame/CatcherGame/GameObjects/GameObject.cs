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

using CatcherGame.GameStates;
using CatcherGame.GameStates.Screen;
using CatcherGame.TextureManager;
namespace CatcherGame.GameObjects
{
    /// <summary>
    /// 遊戲物件的基礎類別
    /// </summary>
    public abstract class GameObject
    {
        protected GameScreen gameScreen; //知道物件目前所屬的遊戲狀態
        
        protected int id; //物件編號
        protected float x, y; //物件座標
        protected float width, height; //物件的寬高

        public GameObject() { 
        
        }

        public GameObject(GameScreen gameScreen, int objId, float x, float y)
        {
            this.gameScreen = gameScreen;
            this.id = objId;
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// 檢查是否有碰撞
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public virtual bool IsCollision(GameObject gameObject)
        {
            return false;
        }
        protected abstract void Init();
        public abstract void LoadResource(TexturesKeyEnum key);
        /// <summary>
        /// 繪製物件圖片
        /// </summary>
        public abstract void Draw(SpriteBatch spriteBatch);

        /// <summary>
        /// 更新物件的邏輯
        /// </summary>
        public abstract void Update();

        #region 屬性

        public float X
        {
            get { return this.x; }
            set { this.x = value; }
        }

        public float Y
        {
            get { return this.y; }
            set { this.y = value; }
        }

        public float Width
        {
            get { return this.width; }
            set { this.width = value; }
        }

        public float Height
        {
            get { return this.height; }
            set { this.height = value; }
        }

        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
        
        #endregion
    }
}