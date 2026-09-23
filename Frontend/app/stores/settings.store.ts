// app/stores/settings.store.ts
import { defineStore } from 'pinia';
import { fetchApi } from '~/services/api';

export interface SystemSetting {
  key: string;
  value: string;
  updatedAt: string;
}

export const useSettingsStore = defineStore('settingsStore', () => {
  const settings = ref<SystemSetting[]>([]);
  const loading = ref(false);

  const fetchSettings = async () => {
    loading.value = true;
    try {
      settings.value = await fetchApi<SystemSetting[]>('/api/settings');
    } finally {
      loading.value = false;
    }
  };

  const getValue = (key: string, fallback = '') =>
    settings.value.find(s => s.key === key)?.value ?? fallback;

  const upsertSetting = async (key: string, value: string) => {
    const updated = await fetchApi<SystemSetting>('/api/settings', {
      method: 'PUT',
      body: { key, value },
    });
    const idx = settings.value.findIndex(s => s.key === key);
    if (idx >= 0) settings.value[idx] = updated;
    else settings.value.push(updated);
  };

  return {
    settings,
    loading,
    fetchSettings,
    getValue,
    upsertSetting,
  };
});
