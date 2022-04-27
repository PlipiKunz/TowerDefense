using CS5410.Particles;
using Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Systems;

namespace CS5410.TowerDefenseGame
{
    public class GameModel
    {
        public static int score;
        public bool isDone;
        public static int health;
        public static int funds;

        private const int GRID_SIZE = 11;
        private readonly int WINDOW_WIDTH;
        private readonly int WINDOW_HEIGHT;

        public static  List<Entity> m_removeThese = new List<Entity>();
        public static  List<Entity> m_addThese = new List<Entity>();

        private Systems.CoordinateSystem m_coordinateSystem;
        private Systems.Renderer m_sysRenderer;
        private Systems.MouseHandeler m_sysMouseHandeler;
        private Systems.CreepMovement m_sysCreepMovement;
        private Systems.KeyboardInput m_sysKeyboardInput;
        private Systems.TowerSystem m_towerSystem;
        private Systems.BulletSystem m_bulletSystem;
        private Systems.LevelSystem m_levelSystem;
        private Systems.SelectionSystem m_selectionSystem;

        public GameModel(int width, int height)
        {
            this.WINDOW_WIDTH = width;
            this.WINDOW_HEIGHT = height;
        }

        public void initialize(ContentManager content, SpriteBatch spriteBatch, GraphicsDevice gd)
        {
            isDone = false;
            score = 0;
            health = 10;
            funds = 100;

            Systems.CoordinateSystem.reset();
            m_coordinateSystem = Systems.CoordinateSystem.Instance();
            m_coordinateSystem.initialize(WINDOW_WIDTH, WINDOW_HEIGHT, GRID_SIZE);

            Systems.CreepMovement.reset();
            m_sysCreepMovement = Systems.CreepMovement.Instance();

            m_sysRenderer = new Systems.Renderer(content, spriteBatch, gd);
            m_sysMouseHandeler = new Systems.MouseHandeler();
            m_towerSystem = new Systems.TowerSystem();
            m_sysKeyboardInput = new Systems.KeyboardInput();
            m_bulletSystem = new Systems.BulletSystem();

            Systems.SelectionSystem.reset();
            m_selectionSystem = Systems.SelectionSystem.Instance();


            Systems.LevelSystem.reset();
            m_levelSystem = Systems.LevelSystem.Instance();

            Renderer.play();

            init(content);
        }

        public void update(GameTime gameTime)
        {

            m_coordinateSystem.Update(gameTime);
            m_sysKeyboardInput.Update(gameTime);
            m_sysCreepMovement.Update(gameTime);
            m_sysMouseHandeler.Update(gameTime);
            m_towerSystem.Update(gameTime);
            m_bulletSystem.Update(gameTime);
            m_levelSystem.Update(gameTime);
            m_selectionSystem.Update(gameTime);


            foreach (var entity in m_removeThese)
            {
                RemoveEntity(entity);
            }
            m_removeThese.Clear();

            foreach (var entity in m_addThese)
            {
                AddEntity(entity);
            }
            m_addThese.Clear();

            if (health <= 0 || (LevelSystem.level == 10 && !LevelSystem.inLevel) )
            {
                isDone = true;
            }

        }

        public void render(GameTime gameTime)
        {
            m_sysRenderer.Update(gameTime);
        }

        public void AddEntity(Entity entity)
        {
            m_coordinateSystem.Add(entity);
            m_sysKeyboardInput.Add(entity);
            m_sysCreepMovement.Add(entity);
            m_sysMouseHandeler.Add(entity);
            m_sysRenderer.Add(entity);
            m_towerSystem.Add(entity);
            m_bulletSystem.Add(entity);
            m_levelSystem.Add(entity);
            m_selectionSystem.Add(entity);

        }

        public void RemoveEntity(Entity entity)
        {
            m_coordinateSystem.Remove(entity.Id);
            m_sysKeyboardInput.Remove(entity.Id);
            m_sysCreepMovement.Remove(entity.Id);
            m_sysMouseHandeler.Remove(entity.Id);
            m_sysRenderer.Remove(entity.Id);
            m_towerSystem.Remove(entity.Id);
            m_bulletSystem.Remove(entity.Id);
            m_levelSystem.Remove(entity.Id);
            m_selectionSystem.Remove(entity.Id);
        }

        private void init(ContentManager content)
        {
            Bullet.init(content);
            Creeps.init(content);
            MouseEntity.init(content);
            Tower.init(content);
            MenuItem.init(content);
            box.init(content);

            Entity curBox;
            for (int i = 0; i < CoordinateSystem.GRID_SIZE; i++){
                for (int j = 0; j < CoordinateSystem.GRID_SIZE; j++)
                {
                    bool goalPos = false;
                    Vector2 pos = new Vector2(i, j);
                    if (m_levelSystem.entrancesAndExits.Contains(pos)) {
                        goalPos = true;
                    }

                    curBox = box.createSimple(i, j, goalPos);
                    AddEntity(curBox);
                }
            }

            var mouse = MouseEntity.create(4, 4);
            AddEntity(mouse);

        }

    }
}