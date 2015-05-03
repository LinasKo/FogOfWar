package com.fog.screens;

import java.util.HashMap;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.Screen;
import com.badlogic.gdx.graphics.GL20;
import com.badlogic.gdx.graphics.OrthographicCamera;
import com.badlogic.gdx.maps.tiled.TiledMap;
import com.badlogic.gdx.maps.tiled.TiledMapTile;
import com.badlogic.gdx.maps.tiled.TiledMapTileLayer;
import com.badlogic.gdx.maps.tiled.TiledMapTileLayer.Cell;
import com.badlogic.gdx.maps.tiled.TmxMapLoader;
import com.badlogic.gdx.maps.tiled.renderers.OrthogonalTiledMapRenderer;
import com.fog.structures.Building;
import com.fog.structures.Resource;
import com.fog.structures.Structure;

public class GameScreen implements Screen {

	private static float UNIT_SCALE = 1 / 32f; // used by camera
	private static int PLAYER;
	private static int PLAYER_FOG_LAYER;

	private TiledMap map;
	private OrthogonalTiledMapRenderer renderer;
	private OrthographicCamera camera;

	private HashMap<String, Resource> resources;
	private HashMap<String, Building> buildings;

	// fogLayerT is the layer of the fog, where the top left tile is
	// transparent.
	// fogLayerO is the layer of the fog, where the top left tile is opaque.
	// both of these are needed for the initialization of FogManipulator.
	public GameScreen(String pathToMap, int player, int fog_layer, int fogLayerT, int fogLayerO) {
		// load the map
		map = new TmxMapLoader().load(pathToMap);
		renderer = new OrthogonalTiledMapRenderer(map, UNIT_SCALE);

		// sort out player specifications
		PLAYER = player;
		PLAYER_FOG_LAYER = fog_layer;

		// find resources and structures
		resources = new HashMap<>();
		buildings = new HashMap<>();
		TiledMapTileLayer objectLayer = (TiledMapTileLayer) map.getLayers().get(1);
		for (int x = 0; x < objectLayer.getWidth(); x++) {
			for (int y = 0; y < objectLayer.getHeight(); y++) {
				Cell cell = objectLayer.getCell(x, y);
				if (cell != null) {
					TiledMapTile tile = cell.getTile();
					String property = (String) tile.getProperties().get("ObjectType");
					switch (property) {
					case ("Wood"):
						resources.put(property, new Resource(tile, Structure.WOOD, Resource.BASE_WOOD));
						break;
					case ("PlayerOneBuilding"):
						buildings.put("PlayerOneBuilding", new Building(tile, Structure.BUILDING, 1, 100));
						break;
					case ("PlayerTwoBuilding"):
						buildings.put("PlayerTwoBuilding", new Building(tile, Structure.BUILDING, 2, 100));
						break;
					}
				}
			}
		}

		// initialize the fog manipulator
		FogManipulator fogManipulator = new FogManipulator(
				((TiledMapTileLayer) map.getLayers().get(fogLayerT)).getCell(0, 0), ((TiledMapTileLayer) map
						.getLayers().get(fogLayerO)).getCell(0, 0));
		
		// create the camera, showing 60x40 units of the world
		camera = new OrthographicCamera();
		camera.setToOrtho(false, 60, 40);
		camera.update();

		// Tips for better performance:
		/*
		 * Only use tiles from a single tile set in a layer. This will reduce
		 * texture binding. Mark tiles that do not need blending as opaque. At
		 * the moment you can only do this programmatically, we will provide
		 * ways to do it in the editor or automatically. Do not go overboard
		 * with the number of layers.
		 */
	}

	@Override
	public void render(float delta) {
		Gdx.gl.glClearColor(0.1f, 0.1f, 0.1f, 1);
		Gdx.gl.glClear(GL20.GL_COLOR_BUFFER_BIT);
		renderer.setView(camera);
		renderer.render(new int[] { 0, 1, 2});
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
		renderer.dispose();
		map.dispose();
	}
}
