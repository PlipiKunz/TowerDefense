using Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;


namespace CS5410.TowerDefenseGame
{
    public class GameModel
    {
        public int score;
        public bool isDone;

        private const int GRID_SIZE = 10;
        private readonly int WINDOW_WIDTH;
        private readonly int WINDOW_HEIGHT;

        public static  List<Entity> m_removeThese = new List<Entity>();
        public static  List<Entity> m_addThese = new List<Entity>();

        private Systems.CoordinateSystem m_coordinateSystem;
        private Systems.Renderer m_sysRenderer;
        private Systems.MouseHandeler m_sysMouseHandeler;
        private Systems.CreepMovement m_sysCreepMovement;
        private Systems.KeyboardInput m_sysKeyboardInput;

        public GameModel(int width, int height)
        {
            this.WINDOW_WIDTH = width;
            this.WINDOW_HEIGHT = height;
        }

        public void initialize(ContentManager content, SpriteBatch spriteBatch)
        {
            isDone = false;
            score = 0;

            var texSquare = content.Load<Texture2D>("Sprites/SquareSprite");
            m_coordinateSystem = new Systems.CoordinateSystem(WINDOW_WIDTH, WINDOW_HEIGHT, GRID_SIZE);

            m_sysRenderer = new Systems.Renderer(spriteBatch, texSquare);
            m_sysMouseHandeler = new Systems.MouseHandeler();

            m_sysCreepMovement = Systems.CreepMovement.Instance();
            Systems.CreepMovement.reset();

            m_sysKeyboardInput = new Systems.KeyboardInput();

            init(texSquare);
        }

        public void update(GameTime gameTime)
        {
            m_coordinateSystem.Update(gameTime);
            m_sysKeyboardInput.Update(gameTime);
            m_sysCreepMovement.Update(gameTime);
            m_sysMouseHandeler.Update(gameTime);

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
            m_sysKeyboardInput.Add(entity);
            m_sysCreepMovement.Add(entity);
            m_sysMouseHandeler.Add(entity);
            m_sysRenderer.Add(entity);
        }

        private void RemoveEntity(Entity entity)
        {
            m_sysKeyboardInput.Remove(entity.Id);
            m_sysCreepMovement.Remove(entity.Id);
            m_sysMouseHandeler.Remove(entity.Id);
            m_sysRenderer.Remove(entity.Id);
        }

        private void init(Texture2D square)
        {
            var mouse = MouseEntity.create(square, 4, 4);
            AddEntity(mouse);


            var tower = SimpleTower.create(square, 8, 9);
            AddEntity(tower);
             tower = SimpleTower.create(square, 7, 9);
            AddEntity(tower);
             tower = SimpleTower.create(square, 9, 7);
            AddEntity(tower);
             tower = SimpleTower.create(square, 8, 7);
            AddEntity(tower);



            var creep = SimpleCreep.create(square, 0, 0, new Vector2(9,9) );
            AddEntity(creep);
        }



    }
}