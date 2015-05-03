package com.fog.screens;

import com.badlogic.gdx.maps.tiled.TiledMapTile;
import com.badlogic.gdx.maps.tiled.TiledMapTileLayer.Cell;

public class FogManipulator {

	TiledMapTile clearTile;
	TiledMapTile foggyTile;

	public FogManipulator(Cell whereTransparent, Cell whereOpaque) {
		clearTile = whereTransparent.getTile();
		foggyTile = whereOpaque.getTile();
		whereTransparent.setTile(foggyTile);
	}

	private void liftFog(Cell[] cells) {
		for (Cell cell : cells) {
			cell.setTile(foggyTile);
		}
	}

	private void hideLand(Cell[] cells) {
		for (Cell cell : cells) {
			cell.setTile(clearTile);
		}
	}
}
