// app/stores/releases.store.ts
import { defineStore } from 'pinia';
import type { Release } from '~/types';
import { fetchApi } from '~/services/api';

export const useReleaseStore = defineStore('releaseStore', () => {
  const releases = ref<Release[]>([]);
  const loading = ref<boolean>(false);
  const activeFilter = ref<string>('All');

  const fetchReleases = async () => {
    loading.value = true;
    try {
      releases.value = await fetchApi<Release[]>('/api/releases');
    } catch (err) {
      console.error('Failed to load releases:', err);
    } finally {
      loading.value = false;
    }
  };

  const filteredReleases = computed(() => {
    if (activeFilter.value === 'All') return releases.value;
    return releases.value.filter(r => r.formats.toLowerCase().includes(activeFilter.value.toLowerCase()));
  });

  const createRelease = async (payload: Omit<Release, 'id' | 'createdAt'>) => {
    const created = await fetchApi<Release>('/api/releases', {
      method: 'POST',
      body: payload,
    });
    releases.value.unshift(created);
    return created;
  };

  const updateRelease = async (id: string, payload: Omit<Release, 'id' | 'createdAt'>) => {
    const updated = await fetchApi<Release>(`/api/releases/${id}`, {
      method: 'PUT',
      body: { id, ...payload },
    });
    const index = releases.value.findIndex(release => release.id === id);
    if (index >= 0) releases.value[index] = updated;
    return updated;
  };

  const deleteRelease = async (id: string) => {
    await fetchApi(`/api/releases/${id}`, { method: 'DELETE' });
    releases.value = releases.value.filter(release => release.id !== id);
  };

  return {
    releases,
    loading,
    activeFilter,
    fetchReleases,
    filteredReleases,
    createRelease,
    updateRelease,
    deleteRelease,
  };
});