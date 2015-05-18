package com.fog.screens;

import aurelienribon.tweenengine.Tween;
import aurelienribon.tweenengine.TweenManager;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.Screen;
import com.badlogic.gdx.graphics.Texture;
import com.badlogic.gdx.graphics.g2d.Sprite;
import com.badlogic.gdx.graphics.g2d.SpriteBatch;
import com.fog.FogOfWar;
import com.fog.tween.SpriteAccessor;

/**
 * Displays the initial splash screen. Creates a GameScreen once the finished.
 * 
 * @author Linasko
 */
public class Splash implements Screen {

	private SpriteBatch spriteBatch;
	private Sprite splashSprite;
	private TweenManager tweenManager;

	FogOfWar game;

	public Splash(FogOfWar game) {
		this.game = game;
	}

	@Override
	public void show() {
		spriteBatch = new SpriteBatch();
		tweenManager = new TweenManager();
		Tween.registerAccessor(Sprite.class, new SpriteAccessor());

		Texture splashTexture = new Texture("img/splash.jpg");
		splashSprite = new Sprite(splashTexture);
		splashSprite.setSize(Gdx.graphics.getWidth(), Gdx.graphics.getHeight());

		Tween.set(splashSprite, SpriteAccessor.ALPHA).target(0).start(tweenManager);
		Tween.to(splashSprite, SpriteAccessor.ALPHA, 2).target(1).start(tweenManager);
		Tween.to(splashSprite, SpriteAccessor.ALPHA, 2).target(0).delay(3).start(tweenManager);
	}

	@Override
	public void render(float delta) {
		Gdx.gl.glClearColor(0, 0, 0, 1);
		Gdx.gl.glClear(Gdx.gl20.GL_COLOR_BUFFER_BIT);

		tweenManager.update(delta);

		spriteBatch.begin();
		splashSprite.draw(spriteBatch);
		spriteBatch.end();

		// If all tweens have been completed, switch to GameScreen
		if (tweenManager.getRunningTweensCount() == 0) {
			game.setScreen(new GameScreen(FogOfWar.PATH_TO_MAP, 1, 2, 2, 3));
		}

	}

	@Override
	public void resize(int width, int height) {
		// TODO Auto-generated method stub

	}

	@Override
	public void pause() {
		// TODO Auto-generated method stub

	}

	@Override
	public void resume() {
		// TODO Auto-generated method stub

	}

	@Override
	public void hide() {
		// TODO Auto-generated method stub

	}

	@Override
	public void dispose() {
		// TODO Auto-generated method stub

	}

}
