using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavesMG : MiniGame
{

    public float threashold = 8;

    Wave[] waves;

    public override void Title()
    {
        StartGame();
    }

    public override void NewGame()
    {
        waves = new Wave[2];

        waves[0] = new Wave(this, 0);
        waves[1] = new Wave(this, Mathf.PI);
    }

    public override void Draw()
    {
        for(int i = 0; i < waves.Length; i++)
        {
            waves[i].draw();
        }
    }

    public class Wave
    {
        float T=0;
        float offset = 0;
        WavesMG game;

        float[] fs;

        public Wave(WavesMG G, float offset)
        {
            game = G;
            this.offset = offset;
            if (offset > 0) T += 16;

            fs = new float[game.R.width / 64 + 1];

            for(int i =0; i < fs.Length; i++)
            {
                fs[i] = Random.Range(0,3);
                if (i == 0) fs[i] = 0;
                if (i == fs.Length-1) fs[i] = 0;
            }
        }

        public void draw()
        {
            // T = (T + game.scrollSpeed) % 32;
            T = 16 + 16 * Mathf.Sin(game.frameCount / 50f + offset);

            float y = -8 + 24 * Mathf.Sin(game.frameCount / 30f + offset);
            float h = 64 + 60 * Mathf.Cos(game.frameCount / 30f + offset + 1f);
            if (h < 64) h = 64;

            for(int i = 0; i < game.R.width/64 + 1; i++)
            {
                if (y > game.threashold) game.R.lset(1);
                else game.R.lset(2);

                fs[i] += 0.03f;
                fs[i] %= 4;

                game.R.spr(Mathf.Floor(fs[i])*32, 0, -T + i*64, 32 + y, 32, 64, false, 64, h);

                game.R.lset(0);

                game.R.spr(128, 0, -T + i*64, 32 + y, 32, 64, false, 64, h);
            }

            //game.R.spr(0, 0, -T, 32 + y, 32, 64, false, 32, h);
            //game.R.spr(0, 0, -T + 32, 32 + y, 32, 64, false, 32, h);

            //game.R.lset(0);

            //game.R.spr(32, 0, -T, 32 + y, 32, 64, false, 32, h);
            //game.R.spr(32, 0, -T + 32, 32 + y, 32, 64, false, 32, h);
        }
    }

}
