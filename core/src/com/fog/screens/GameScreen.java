package com.fog.screens;

import java.util.HashMap;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.Screen;
import com.badlogic.gdx.graphics.GL20;
import com.badlogic.gdx.graphics.OrthographicCamera;
import com.badlogic.gdx.maps.MapLayers;
import com.badlogic.gdx.maps.tiled.TiledMap;
import com.badlogic.gdx.maps.tiled.TiledMapTile;
import com.badlogic.gdx.maps.tiled.TiledMapTileLayer;
import com.badlogic.gdx.maps.tiled.TiledMapTileLayer.Cell;
import com.badlogic.gdx.maps.tiled.TmxMapLoader;
import com.badlogic.gdx.maps.tiled.renderers.OrthogonalTiledMapRenderer;
import com.fog.structures.Building;
import com.fog.structures.Resource;
import com.fog.structures.Structure;
import com.fog.tools.FogManipulator;

/**
 * GameScreen, shown to a particular player
 * 
 * @author Linasko
 */
public class GameScreen implements Screen {

	private static float UNIT_SCALE = 1 / 32f; // Scale. Used by camera. TODO -
												// find out what this is.
	private static int PLAYER; // Player number.
	private static int PLAYER_FOG_LAYER; // Number of the layer with fog for
											// this player.
	public static int NUMBER_OF_PLAYERS = 2; // number of players.

	private TiledMap map;
	private OrthogonalTiledMapRenderer renderer;
	private OrthographicCamera camera;

	private HashMap<String, Resource> resources; // All natural resources on the
													// map.
	private HashMap<String, Building> buildings; // All buildings on the map.

	// TODO: expand entries in hash map to arrays, instead of particular objects

	// fogLayerT is the layer of the fog, where the top left tile is
	// transparent.
	// fogLayerO is the layer of the fog, where the top left tile is opaque.
	// both of these are needed for the initialization of FogManipulator.
	public GameScreen(String pathToMap, int player, int player_fog_layer, int fogLayerT, int fogLayerO) {
		// load the map
		map = new TmxMapLoader().load(pathToMap);
		renderer = new OrthogonalTiledMapRenderer(map, UNIT_SCALE);

		// sort out player specifications
		PLAYER = player;
		PLAYER_FOG_LAYER = player_fog_layer;

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
						resources.put(property, new Resource(tile, x, y, Structure.WOOD, Resource.BASE_WOOD));
						break;
					case ("PlayerOneBuilding"):
						buildings.put("PlayerOneBuilding", new Building(tile, x, y, Structure.BUILDING, 0, 100, 6f));
						break;
					case ("PlayerTwoBuilding"):
						buildings.put("PlayerTwoBuilding", new Building(tile, x, y, Structure.BUILDING, 1, 100, 6f));
						break;
					}
				}
			}
		}

		// Initialize the fog manipulator. Assuming that there are 2 non-fog
		// layers and they are in the beginning.
		MapLayers foggy = new MapLayers();
		for (int i = 2; i < map.getLayers().getCount(); i++) {
			foggy.add(map.getLayers().get(i));
		}
		FogManipulator.initialize(((TiledMapTileLayer) map.getLayers().get(fogLayerT)).getCell(0, 0),
				((TiledMapTileLayer) map.getLayers().get(fogLayerO)).getCell(0, 0), NUMBER_OF_PLAYERS, foggy);

		// Reveal castles.
		Building castle1 = buildings.get("PlayerOneBuilding");
		Building castle2 = buildings.get("PlayerTwoBuilding");
		FogManipulator.revealCircle(0, castle1.getX(), castle1.getY(), castle1.getSight_range());
		FogManipulator.revealCircle(1, castle2.getX(), castle2.getY(), castle2.getSight_range());

		// create the camera, showing 30x20 units of the world
		camera = new OrthographicCamera();
		camera.setToOrtho(false, 25, 50);
		camera.update();

		// Tips for better performance: (taken from the Internet)
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
		// Set view to where the camera is currently facing.
		renderer.setView(camera);
		// Render the specified layers.
		renderer.render(new int[] { 0, 1, 2 });
	}

	@Override
	public void resize(int width, int height) {
	}

	@Override
	public void show() {
	}

	@Override
	public void hide() {
	}

	@Override
	public void pause() {
	}

	@Override
	public void resume() {
	}

	@Override
	public void dispose() {
		renderer.dispose();
		map.dispose();
	}
}
