import { Injectable } from '@angular/core';

/**
 * Lightweight wrapper around the Cache Storage API for storing JSON payloads.
 * Falls back silently when Cache API is not available (SSR or older browsers).
 */
@Injectable({ providedIn: 'root' })
export class CacheStorageService {
  private readonly defaultCacheName = 'app-cache';

  private isAvailable(): boolean {
    return typeof window !== 'undefined' && 'caches' in window;
  }

  async putJson(
    cacheName: string = this.defaultCacheName,
    key: string,
    value: any
  ): Promise<void> {
    if (!this.isAvailable()) return;
    try {
      const cache = await caches.open(cacheName);
      const url = this.keyToUrl(key);
      const body = JSON.stringify(value);
      const response = new Response(body, {
        headers: { 'Content-Type': 'application/json; charset=utf-8' },
      });
      await cache.put(url, response);
    } catch (e) {
      // fail silently - not critical
      console.warn('CacheStorageService.putJson failed', e);
    }
  }

  async matchJson<T = any>(
    cacheName: string = this.defaultCacheName,
    key: string
  ): Promise<T | null> {
    if (!this.isAvailable()) return null;
    try {
      const cache = await caches.open(cacheName);
      const url = this.keyToUrl(key);
      const resp = await cache.match(url);
      if (!resp) return null;
      const text = await resp.text();
      return JSON.parse(text) as T;
    } catch (e) {
      console.warn('CacheStorageService.matchJson failed', e);
      return null;
    }
  }

  async deleteEntry(
    cacheName: string = this.defaultCacheName,
    key: string
  ): Promise<void> {
    if (!this.isAvailable()) return;
    try {
      const cache = await caches.open(cacheName);
      const url = this.keyToUrl(key);
      await cache.delete(url);
    } catch (e) {
      console.warn('CacheStorageService.deleteEntry failed', e);
    }
  }

  async listKeys(cacheName: string = this.defaultCacheName): Promise<string[]> {
    if (!this.isAvailable()) return [];
    try {
      const cache = await caches.open(cacheName);
      const keys = await cache.keys();
      return keys.map((k) => k.url);
    } catch (e) {
      console.warn('CacheStorageService.listKeys failed', e);
      return [];
    }
  }

  private keyToUrl(key: string): string {
    // Use an opaque pseudo-URL so we can store arbitrary keys
    return `https://app.local/cache/${encodeURIComponent(key)}`;
  }
}
