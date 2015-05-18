package com.fog.screens;

import com.badlogic.gdx.maps.tiled.TiledMapTile;
import com.badlogic.gdx.maps.tiled.TiledMapTileLayer;
import com.badlogic.gdx.maps.tiled.TiledMapTileLayer.Cell;

/**
 * FogManipulator is used to control the fog.
 * 
 * @author Linasko
 */
public class FogManipulator {

	// TODO convert FogManipulator to static, as it is only initialized once.
	private TiledMapTile clearTile;
	private TiledMapTile foggyTile;

	// Holds all fog values for a particular player.
	// 0 == foggy
	// 1 == hidden
	// 2 == clear
	public int[][] cell_states;

	/**
	 * FogManipulator
	 * 
	 * @param whereTransparent
	 *            - Initially there must be at least one transparent tile on the
	 *            map. A cell, containing that tile should be passed to the
	 *            manipualtor.
	 * @param whereOpaque
	 *            - Likewise, there should be a foggy tile, contained in a cell
	 *            that should be passed to the manipulator.
	 * @param width
	 *            - the width of the map.
	 * @param height
	 *            - the height of the map.
	 */
	public FogManipulator(Cell whereTransparent, Cell whereOpaque, int width, int height) {
		cell_states = new int[height][width];
		// Keep foggy and clear tile for future usage in covering and uncovering
		// of the fog. Then hide the clear tile. It is assumed that the whole
		// map is cloudy initially.
		// TODO initialize tiles differently as this can cause problems.
		clearTile = whereTransparent.getTile();
		foggyTile = whereOpaque.getTile();
		hideTile(whereTransparent, 0, 0);
	}

	/**
	 * Clears fog in a cell
	 * 
	 * @param cell
	 * @param x
	 * @param y
	 */
	private void revealTile(Cell cell, int x, int y) {
		cell_states[y][x] = 2;
		cell.setTile(clearTile);
	}

	/**
	 * Creates fog in a cell
	 * 
	 * @param cell
	 * @param x
	 * @param y
	 */
	private void hideTile(Cell cell, int x, int y) {
		cell_states[y][x] = 0;
		cell.setTile(foggyTile);
	}

	/**
	 * Creates fog in a circle around a point.
	 * 
	 * @param layer
	 *            where the fog will be created
	 * @param x
	 *            coordinate of the point
	 * @param y
	 *            coordinate of the point
	 * @param radius
	 *            of the circle
	 */
	public void hideCircle(TiledMapTileLayer layer, int x, int y, float radius) {
		for (int cell_x = 0; cell_x < cell_states[0].length; cell_x++) {
			for (int cell_y = 1; cell_y < cell_states.length; cell_y++) {
				if (cell_states[cell_y][cell_x] != 0
						&& Math.sqrt(x - cell_x) + Math.sqrt(y - cell_y) < Math.sqrt(radius)) {
					hideTile(layer.getCell(cell_x, cell_y), cell_x, cell_y);
				}
			}
		}
	}

	/**
	 * Clears fog in a circle around a point.
	 * 
	 * @param layer
	 *            where the fog will be cleared
	 * @param x
	 *            coordinate of the point
	 * @param y
	 *            coordinate of the point
	 * @param radius
	 *            of the circle
	 */
	public void revealCircle(TiledMapTileLayer layer, int x, int y, float radius) {
		for (int cell_x = 0; cell_x < cell_states[0].length; cell_x++) {
			for (int cell_y = 0; cell_y < cell_states.length; cell_y++) {
				if (cell_states[cell_y][cell_x] != 2
						&& (x - cell_x) * (x - cell_x) + (y - cell_y) * (y - cell_y) < radius * radius) {
					revealTile(layer.getCell(cell_x, cell_y), cell_x, cell_y);
				}
			}
		}
	}
}
