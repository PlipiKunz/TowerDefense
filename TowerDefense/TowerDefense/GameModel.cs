using CS5410.Particles;
using Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;


namespace CS5410.TowerDefenseGame
{
    public class GameModel
    {
        public static int score;
        public bool isDone;
        public static int health;
        public static int funds;
        public static int level;

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

        public GameModel(int width, int height)
        {
            this.WINDOW_WIDTH = width;
            this.WINDOW_HEIGHT = height;
        }

        public void initialize(ContentManager content, SpriteBatch spriteBatch, GraphicsDevice gd)
        {
            isDone = false;
            score = 0;
            level = 0;
            health = 1;
            funds = 100;


            Systems.CoordinateSystem.reset();
            m_coordinateSystem = Systems.CoordinateSystem.Instance();
            m_coordinateSystem.initialize(WINDOW_WIDTH, WINDOW_HEIGHT, GRID_SIZE);

            Systems.CreepMovement.reset();
            m_sysCreepMovement = Systems.CreepMovement.Instance();

            m_sysRenderer = new Systems.Renderer(content, spriteBatch, gd, new ParticleEmitter(content, 5, 2, new TimeSpan(0, 0, 1)));
            m_sysMouseHandeler = new Systems.MouseHandeler();
            m_towerSystem = new Systems.TowerSystem();
            m_sysKeyboardInput = new Systems.KeyboardInput();
            m_bulletSystem = new Systems.BulletSystem();

            init(content);
        }

        public void update(GameTime gameTime)
        {
            if (health <= 0) {
                isDone = true;
            }

            m_coordinateSystem.Update(gameTime);
            m_sysKeyboardInput.Update(gameTime);
            m_sysCreepMovement.Update(gameTime);
            m_sysMouseHandeler.Update(gameTime);
            m_towerSystem.Update(gameTime);
            m_bulletSystem.Update(gameTime);

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
        }

        public void render(GameTime gameTime)
        {
            m_sysRenderer.Update(gameTime);
        }

        private void AddEntity(Entity entity)
        {
            m_coordinateSystem.Add(entity);
            m_sysKeyboardInput.Add(entity);
            m_sysCreepMovement.Add(entity);
            m_sysMouseHandeler.Add(entity);
            m_sysRenderer.Add(entity);
            m_towerSystem.Add(entity);
            m_bulletSystem.Add(entity);
        }

        private void RemoveEntity(Entity entity)
        {
            m_coordinateSystem.Remove(entity.Id);
            m_sysKeyboardInput.Remove(entity.Id);
            m_sysCreepMovement.Remove(entity.Id);
            m_sysMouseHandeler.Remove(entity.Id);
            m_sysRenderer.Remove(entity.Id);
            m_towerSystem.Remove(entity.Id);
            m_bulletSystem.Remove(entity.Id);
        }

        private void init(ContentManager content)
        {
            SimpleBullet.init(content);
            SimpleCreep.init(content);
            MouseEntity.init(content);
            SimpleTower.init(content);
            

            var mouse = MouseEntity.create(4, 4);
            AddEntity(mouse);

            var tower = SimpleTower.create( 2, 2);
            AddEntity(tower);
             tower = SimpleTower.create(4, 4);
            AddEntity(tower);
             tower = SimpleTower.create(9, 7);
            AddEntity(tower);
             tower = SimpleTower.create( 10, 6);
            AddEntity(tower);

            var creep = SimpleCreep.create( 0, 0, new Vector2(9,9) );
            AddEntity(creep);
        }

    }
}