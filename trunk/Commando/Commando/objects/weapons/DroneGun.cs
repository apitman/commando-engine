using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Commando.collisiondetection;

namespace Commando.objects.weapons
{
    public class DroneGun : WeaponAbstract
    {
        public DroneGun(List<DrawableObjectAbstract> pipeline, CharacterAbstract character, GameTexture animation, Vector2 gunHandle)
            : base(pipeline, character, animation, gunHandle)
        {
            // nothing
        }

        public override void shoot(Commando.collisiondetection.CollisionDetectorInterface detector)
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
                Projectile bullet = new Projectile(TextureMap.fetchTexture("Bullet"), detector, new ConvexPolygon(points, Vector2.Zero), 2.5f, rotation_ * 20.0f, pos, rotation_, 0.5f);
                drawPipeline_.Add(bullet);
                refireCounter_ = 10;
                character_.getAmmo().update(character_.getAmmo().getValue() - 1);


                weaponFired_ = true;
            }
        }
    }
}
