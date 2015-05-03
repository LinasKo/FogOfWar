package com.fog.screens;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.Screen;
import com.badlogic.gdx.graphics.GL20;
import com.badlogic.gdx.graphics.OrthographicCamera;
import com.badlogic.gdx.maps.tiled.TiledMap;
import com.badlogic.gdx.maps.tiled.TmxMapLoader;
import com.badlogic.gdx.maps.tiled.renderers.OrthogonalTiledMapRenderer;

public class GameScreen implements Screen {

	private static float UNIT_SCALE = 1 / 32f;  // used by camera
	
	private TiledMap map;
	private OrthogonalTiledMapRenderer renderer;
	private OrthographicCamera camera;
	
	
	public GameScreen(String pathToMap) {
		// load the map
		map = new TmxMapLoader().load(pathToMap);
		renderer = new OrthogonalTiledMapRenderer(map, UNIT_SCALE);
		
		// create the camera, showing 30x20 units of the world
		camera = new OrthographicCamera();
		camera.setToOrtho(false, 30, 20);
		camera.update();
		
		// Tips for better performance:
		/*
		 * Only use tiles from a single tile set in a layer. This will reduce texture binding.
		 * Mark tiles that do not need blending as opaque. At the moment you can only do this programmatically, we will provide ways to do it in the editor or automatically.
		 * Do not go overboard with the number of layers.
		 */
	}
	
	@Override
	public void render(float delta) {
		Gdx.gl.glClearColor(0.1f, 0.1f, 0.1f, 1);
		Gdx.gl.glClear(GL20.GL_COLOR_BUFFER_BIT);
		renderer.setView(camera);
		renderer.render(new int[] {0,1});
	}

	@Override
	public void resize(int width, int height) {
		// TODO Auto-generated method stub
	}

	@Override
	public void show() {
		// TODO Auto-generated method stub
	}

	@Override
	public void hide() {
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
	public void dispose() {
		map.dispose();
	}
}
