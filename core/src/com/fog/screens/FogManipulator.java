package com.fog.screens;

import com.badlogic.gdx.maps.tiled.TiledMapTile;
import com.badlogic.gdx.maps.tiled.TiledMapTileLayer;
import com.badlogic.gdx.maps.tiled.TiledMapTileLayer.Cell;

public class FogManipulator {

	private TiledMapTile clearTile;
	private TiledMapTile foggyTile;
	
	// 0 == foggy
	// 1 == hidden
	// 2 == clear
	public int[][] cell_states;

	public FogManipulator(Cell whereTransparent, Cell whereOpaque, int width, int height) {
		cell_states = new int[height][width];
		clearTile = whereTransparent.getTile();
		foggyTile = whereOpaque.getTile();
		hideTile(whereTransparent, 0, 0);
	}

	private void revealTile(Cell cell, int x, int y) {
		cell_states[y][x] = 2;
		cell.setTile(clearTile);
	}

	private void hideTile(Cell cell, int x, int y) {
		cell_states[y][x] = 0;
		cell.setTile(foggyTile);
	}
	
	public void hideCircle(TiledMapTileLayer layer, int x, int y, float radius) {
		for (int cell_x = 0; cell_x < cell_states[0].length; cell_x++) {
			for (int cell_y = 1; cell_y < cell_states.length; cell_y++) {
				if (cell_states[cell_y][cell_x] != 0 && Math.sqrt(x - cell_x) + Math.sqrt(y - cell_y) < Math.sqrt(radius)) {
					hideTile(layer.getCell(cell_x, cell_y), cell_x, cell_y);
				}
			}
		}
	}
	
	public void revealCircle(TiledMapTileLayer layer, int x, int y, float radius) {
		for (int cell_x = 0; cell_x < cell_states[0].length; cell_x++) {
			for (int cell_y = 0; cell_y < cell_states.length; cell_y++) {
				if (cell_states[cell_y][cell_x] != 2 && (x - cell_x)*(x - cell_x) + (y - cell_y)*(y - cell_y) < radius*radius) {
					revealTile(layer.getCell(cell_x, cell_y), cell_x, cell_y);
				}
			}
		}
	}
}
