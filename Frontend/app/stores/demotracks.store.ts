// app/stores/demotracks.store.ts
import { defineStore } from 'pinia';
import type { DemoTrack } from '~/types';
import { fetchApi } from '~/services/api';

export const useDemoTrackStore = defineStore('demoTrackStore', () => {
  const tracks = ref<DemoTrack[]>([]);
  const loading = ref<boolean>(false);
  const activeGenre = ref<string>('All');

  const fetchDemoTracks = async () => {
    loading.value = true;
    try {
      tracks.value = await fetchApi<DemoTrack[]>('/api/demotracks');
    } catch (err) {
      console.error('Failed to load demo tracks:', err);
    } finally {
      loading.value = false;
    }
  };

  const genres = computed(() =>
    Array.from(new Set(tracks.value.map(track => track.genre)))
      .sort((first, second) => first.localeCompare(second, undefined, { sensitivity: 'base' }))
  );

  const filteredTracks = computed(() => {
    if (activeGenre.value === 'All') return tracks.value;
    return tracks.value.filter(t => t.genre === activeGenre.value);
  });

  const createTrack = async (payload: Omit<DemoTrack, 'id'>) => {
    const created = await fetchApi<DemoTrack>('/api/demotracks', {
      method: 'POST',
      body: payload,
    });
    tracks.value.unshift(created);
    return created;
  };

  const updateTrack = async (id: string, payload: Omit<DemoTrack, 'id'>) => {
    const updated = await fetchApi<DemoTrack>(`/api/demotracks/${id}`, {
      method: 'PUT',
      body: { id, ...payload },
    });
    const idx = tracks.value.findIndex(t => t.id === id);
    if (idx >= 0) tracks.value[idx] = updated;
    return updated;
  };

  const deleteTrack = async (id: string) => {
    await fetchApi(`/api/demotracks/${id}`, { method: 'DELETE' });
    tracks.value = tracks.value.filter(t => t.id !== id);
  };

  return {
    tracks,
    loading,
    activeGenre,
    genres,
    filteredTracks,
    fetchDemoTracks,
    createTrack,
    updateTrack,
    deleteTrack,
  };
});
