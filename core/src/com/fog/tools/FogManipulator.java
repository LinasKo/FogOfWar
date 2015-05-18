package com.fog.tools;

import java.util.ArrayList;
import java.util.HashMap;

import com.badlogic.gdx.maps.MapLayer;
import com.badlogic.gdx.maps.MapLayers;
import com.badlogic.gdx.maps.tiled.TiledMapTile;
import com.badlogic.gdx.maps.tiled.TiledMapTileLayer;
import com.badlogic.gdx.maps.tiled.TiledMapTileLayer.Cell;

/**
 * FogManipulator is used to control the fog.
 * 
 * @author Linasko
 */
public class FogManipulator {

	// declare tiles that will be used in map reveal
	private static TiledMapTile clearTile;
	private static TiledMapTile foggyTile;

	// Holds fog state matrices for all players.
	private static final int FOGGY = 0;
	private static final int HIDDEN = 1;
	private static final int CLEAR = 2;
	public static int[][][] fog_states;
	public static MapLayers fog_layers;

	/**
	 * FogManipulator
	 * 
	 * @param whereTransparent
	 *            - Initially there must be at least one transparent tile on the
	 *            map. A cell, containing that tile should be passed to the
	 *            manipulator.
	 * @param whereOpaque
	 *            - Likewise, there should be a foggy tile, contained in a cell
	 *            that should be passed to the manipulator.
	 * @param width
	 *            - the width of the map.
	 * @param height
	 *            - the height of the map.
	 * @param number_of_players
	 *            - number of players
	 * @param layers
	 *            - all fog layers of the map
	 */
	public static void initialize(Cell whereTransparent, Cell whereOpaque, int number_of_players, MapLayers layers) {
		fog_layers = layers;
		fog_states = new int[number_of_players][((TiledMapTileLayer) layers.get(0)).getHeight()][((TiledMapTileLayer) layers
				.get(0)).getWidth()];
		// Keep foggy and clear tile for future usage in covering and uncovering
		// of the fog. Then hide the clear tile. It is assumed that the whole
		// map is cloudy initially.
		// TODO initialize tiles differently as this can cause problems.
		clearTile = whereTransparent.getTile();
		foggyTile = whereOpaque.getTile();
		whereTransparent.setTile(foggyTile);
	}

	/**
	 * Clears fog in a cell for a particular player
	 * 
	 * @param player
	 * @param x
	 * @param y
	 */
	public static void revealTile(int player, int x, int y) {
		fog_states[player][y][x] = CLEAR;
		((TiledMapTileLayer) fog_layers.get(player)).getCell(x, y).setTile(clearTile);
	}

	/**
	 * Creates fog in a cell for a particular player
	 * 
	 * @param player
	 * @param x
	 * @param y
	 */
	public static void hideTile(int player, int x, int y) {
		fog_states[player][y][x] = HIDDEN;
		((TiledMapTileLayer) fog_layers.get(player)).getCell(x, y).setTile(foggyTile);
	}

	/**
	 * Creates fog in a circle around a point.
	 * 
	 * @param player
	 *            the player number
	 * @param x
	 *            coordinate of the point
	 * @param y
	 *            coordinate of the point
	 * @param radius
	 *            of the circle
	 */
	public static void hideCircle(int player, int x, int y, float radius) {
		for (int cell_x = 0; cell_x < fog_states[player][0].length; cell_x++) {
			for (int cell_y = 0; cell_y < fog_states[player].length; cell_y++) {
				if (fog_states[player][cell_y][cell_x] != HIDDEN
						&& (x - cell_x) * (x - cell_x) + (y - cell_y) * (y - cell_y) < radius * radius) {
					hideTile(player, cell_x, cell_y);
				}
			}
		}
	}

	/**
	 * Clears fog in a circle around a point.
	 * 
	 * @param player
	 *            the player number
	 * @param x
	 *            coordinate of the point
	 * @param y
	 *            coordinate of the point
	 * @param radius
	 *            of the circle
	 */
	public static void revealCircle(int player, int x, int y, float radius) {
		for (int cell_x = 0; cell_x < fog_states[player][0].length; cell_x++) {
			for (int cell_y = 0; cell_y < fog_states[player].length; cell_y++) {
				if (fog_states[player][cell_y][cell_x] != CLEAR
						&& (x - cell_x) * (x - cell_x) + (y - cell_y) * (y - cell_y) < radius * radius) {
					revealTile(player, cell_x, cell_y);
				}
			}
		}
	}
}
