using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commando.collisiondetection;
using Microsoft.Xna.Framework;
using Commando.ai;
using Commando.levels;

namespace Commando.objects.weapons
{
    class Pistol : WeaponAbstract
    {
        protected GameTexture laserImage_;
        protected Vector2 laserTarget_;

        public Pistol(List<DrawableObjectAbstract> pipeline, CharacterAbstract character, GameTexture animation, Vector2 gunHandle)
            : base(pipeline, character, animation, gunHandle)
        {
            laserImage_ = TextureMap.fetchTexture("laserpointer");
        }

        public override void shoot(CollisionDetectorInterface detector)
        {
            if (refireCounter_ == 0 && character_.getAmmo().getValue() > 0)
            {
                List<Vector2> points = new List<Vector2>();
                points.Add(new Vector2(2f, 2f));
                points.Add(new Vector2(-2f, 2f));
                points.Add(new Vector2(-2f, -2f));
                points.Add(new Vector2(2f, -2f));
                rotation_.Normalize();
                Vector2 pos = position_ + rotation_ * 15f;
                Projectile bullet = new Projectile(TextureMap.getInstance().getTexture("Bullet"), detector, new ConvexPolygon(points, Vector2.Zero), 2.5f, rotation_ * 20.0f, pos, rotation_, 0.5f);
                drawPipeline_.Add(bullet);
                refireCounter_ = 10;
                character_.getAmmo().update(character_.getAmmo().getValue() - 1);


                weaponFired_ = true;
            }
        }

        public override void update()
        {
            base.update();

            // TODO Change/fix how this is done, modularize it, etc.
            // Essentially, the player updates his visual location in the WorldState
            // Must remove before adding because Dictionaries don't like duplicate keys
            // Removing a nonexistent key (for first frame) does no harm
            // Also, need to make it so the radius isn't hardcoded - probably all
            //  objects which will have a visual stimulus should have a radius
            WorldState.Audial_.Remove(audialStimulusId_);
            if (weaponFired_)
            {
                weaponFired_ = false;
                WorldState.Audial_.Add(
                    audialStimulusId_,
                    new Stimulus(StimulusSource.CharacterAbstract, StimulusType.Position, 150.0f, character_.getPosition())
                );
            }

            laserTarget_ = Raycaster.roughCollision(position_, rotation_, new Height(false, true));
        }

        public override void draw()
        {
            base.draw();
            if (!Settings.getInstance().UsingMouse_)
                laserImage_.drawImage(0, laserTarget_, Constants.DEPTH_LASER);
        }
    }
}
