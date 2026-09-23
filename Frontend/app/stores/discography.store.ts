// app/stores/discography.store.ts
import { defineStore } from 'pinia';
import type { DiscographyItem } from '~/types';
import { fetchApi } from '~/services/api';

export const useDiscographyStore = defineStore('discographyStore', () => {
  const items = ref<DiscographyItem[]>([]);
  const loading = ref<boolean>(false);

  const sortByNewest = () => {
    items.value.sort((first, second) =>
      new Date(second.releaseDate).getTime() - new Date(first.releaseDate).getTime()
    );
  };

  const fetchDiscography = async () => {
    loading.value = true;
    try {
      items.value = await fetchApi<DiscographyItem[]>('/api/discography');
      sortByNewest();
    } catch (err) {
      console.error('Failed to load discography:', err);
    } finally {
      loading.value = false;
    }
  };

  const groupedByYear = computed(() => {
    const groups: Record<string, DiscographyItem[]> = {};
    for (const item of items.value) {
      const year = new Date(item.releaseDate).getFullYear().toString();
      if (!groups[year]) groups[year] = [];
      groups[year].push(item);
    }
    return Object.entries(groups)
      .sort(([firstYear], [secondYear]) => Number(secondYear) - Number(firstYear))
      .map(([year, entries]) => ({ year, entries }));
  });

  const createItem = async (payload: Omit<DiscographyItem, 'id' | 'createdAt'>) => {
    const created = await fetchApi<DiscographyItem>('/api/discography', {
      method: 'POST',
      body: payload,
    });
    items.value.unshift(created);
    sortByNewest();
    return created;
  };

  const updateItem = async (id: string, payload: Omit<DiscographyItem, 'id' | 'createdAt'>) => {
    const updated = await fetchApi<DiscographyItem>(`/api/discography/${id}`, {
      method: 'PUT',
      body: { id, ...payload },
    });
    const idx = items.value.findIndex(i => i.id === id);
    if (idx >= 0) items.value[idx] = updated;
    sortByNewest();
    return updated;
  };

  const deleteItem = async (id: string) => {
    await fetchApi(`/api/discography/${id}`, { method: 'DELETE' });
    items.value = items.value.filter(i => i.id !== id);
  };

  return {
    items,
    loading,
    fetchDiscography,
    groupedByYear,
    createItem,
    updateItem,
    deleteItem,
  };
});
