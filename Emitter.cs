using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace WindowsFormsApp4
{
    public class Emitter
    {
        public float GravitationX = 0;
        public float GravitationY = 0;

        List<Particle> particles = new List<Particle>();

        public int MousePositionX;
        public int MousePositionY;

        public int Speedmin = 1;    // Минимальная скорость падения частиц
        public int SpeedMax = 10;    // Максимальная скорость падения частиц

        public List<IImpactPoint> impactPoints = new List<IImpactPoint>();

        public int ParticlesCount = 1500;  // Количество частиц
        public virtual void ResetParticle(Particle particle)
        {
            particle.Life = 70 + Particle.rand.Next(100);
            particle.X = MousePositionX;
            particle.Y = MousePositionY;

            var direction = (double)Particle.rand.Next(360);
            var speed = Speedmin + Particle.rand.Next(10);

            particle.SpeedX = (float)(Math.Cos(direction / 180 * Math.PI) * speed);
            particle.SpeedY = -(float)(Math.Sin(direction / 180 * Math.PI) * speed);

            particle.Radius = 2 + Particle.rand.Next(10);

            if (particle.Life > 0)
            {
                var color = particle as ParticleColorful;
                color.FromColor = Color.White;
                color.ToColor = Color.White;
            }
        }

        public void UpdateState()
        {
            List<Particle> temp = new List<Particle>();
            foreach (var particle in particles)
            {
                particle.Life -= 1;  
                if (particle.Life < 0)
                {

                    if (particles.Count > ParticlesCount)
                    {
                        temp.Add(particle);
                        
                    } else
                    {
                        ResetParticle(particle);
                    }
                }
                else
                {
                    foreach (var point in impactPoints)
                    {
                        point.ImpactParticle(particle);
                    }

                    particle.SpeedX += GravitationX;
                    particle.SpeedY += GravitationY;

                    particle.X += particle.SpeedX;
                    particle.Y += particle.SpeedY;
                }
            }

            foreach (var t in temp)
            {
                particles.Remove(t);
            }

            for (var i = 0; i < 10; ++i)
            {
                if (particles.Count < ParticlesCount)
                {
                    var particle = new ParticleColorful();
                    particle.FromColor = Color.White;
                    particle.ToColor = Color.FromArgb(0, Color.White);

                    ResetParticle(particle);

                    particles.Add(particle);
                }
                else
                {
                    break;
                }
            }
        }

        public void Render(Graphics g)
        {
            foreach (var particle in particles)
            {
                particle.Draw(g);
            }

            foreach (var point in impactPoints) 
            {
                point.Render(g); 
            }
        }
    }

    public class TopEmitter : Emitter
    {
        public int Width;

        public override void ResetParticle(Particle particle)
        {
            base.ResetParticle(particle); 

            particle.X = Particle.rand.Next(Width);
            particle.Y = 0; 

            particle.SpeedY = Speedmin; 
            
            particle.SpeedX = Particle.rand.Next(-2, 2);
        }
    }
}
