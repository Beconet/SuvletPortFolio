// app/stores/profile.store.ts
import { defineStore } from 'pinia';
import type { ProfileInfo } from '~/types';
import { fetchApi } from '~/services/api';

export const useProfileStore = defineStore('profileStore', () => {
  const profile = ref<ProfileInfo | null>(null);
  const loading = ref<boolean>(false);

  const fetchProfile = async () => {
    if (profile.value) return;
    loading.value = true;
    try {
      profile.value = await fetchApi<ProfileInfo>('/api/profile');
    } catch (err) {
      console.error('Failed to load profile:', err);
    } finally {
      loading.value = false;
    }
  };

  const updateProfile = async (payload: Omit<ProfileInfo, 'id'>) => {
    const updated = await fetchApi<ProfileInfo>('/api/profile', {
      method: 'PUT',
      body: payload,
    });
    profile.value = updated;
    return updated;
  };

  return {
    profile,
    loading,
    fetchProfile,
    updateProfile,
  };
});
