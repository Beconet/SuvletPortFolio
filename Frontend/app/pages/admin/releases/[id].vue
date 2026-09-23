<!-- app/pages/admin/releases/[id].vue -->
<template>
  <div class="max-w-2xl">
    <p class="text-xs uppercase tracking-widest text-[#888] mb-2">RELEASES</p>
    <h1 class="text-4xl font-serif italic mb-8">{{ isNew ? 'New Release' : 'Edit Release' }}</h1>

    <form v-if="isNew || existing" @submit.prevent="handleSubmit" class="flex flex-col gap-5">
      <div>
        <label class="text-xs uppercase tracking-wider text-[#888] mb-2 block">Title</label>
        <input v-model="form.title" required class="input" />
      </div>
      <div>
        <label class="text-xs uppercase tracking-wider text-[#888] mb-2 block">Formats (e.g. Album, EP, Single)</label>
        <input v-model="form.formats" required class="input" />
      </div>
      <div>
        <label class="text-xs uppercase tracking-wider text-[#888] mb-2 block">Release Date</label>
        <input v-model="form.releaseDate" type="date" required class="input" />
      </div>
      <div>
        <label class="text-xs uppercase tracking-wider text-[#888] mb-2 block">Description</label>
        <textarea v-model="form.description" rows="3" class="input"></textarea>
      </div>
      <div>
        <label class="text-xs uppercase tracking-wider text-[#888] mb-2 block">Cover Image</label>
        <div class="flex flex-col sm:flex-row gap-3">
          <input v-model="form.coverImageUrl" placeholder="https://..." class="input flex-1" />
          <label class="border border-[#333] hover:border-[#666] px-4 py-3 text-xs cursor-pointer transition text-center whitespace-nowrap">
            {{ uploading ? 'Uploading...' : 'Upload' }}
            <input type="file" accept="image/*" class="hidden" @change="onFileChange($event, 'coverImageUrl')" />
          </label>
        </div>
      </div>
      <div>
        <label class="text-xs uppercase tracking-wider text-[#888] mb-2 block">Spotify URL</label>
        <input v-model="form.spotifyUrl" class="input" />
      </div>
      <div>
        <label class="text-xs uppercase tracking-wider text-[#888] mb-2 block">Apple Music URL</label>
        <input v-model="form.appleMusicUrl" class="input" />
      </div>
      <div>
        <label class="text-xs uppercase tracking-wider text-[#888] mb-2 block">YouTube URL</label>
        <input v-model="form.youtubeUrl" class="input" />
      </div>

      <p v-if="error" class="text-xs text-red-400">{{ error }}</p>

      <div class="flex gap-3 mt-2">
        <button type="submit" :disabled="submitting" class="bg-[#ccff00] text-[#0a0a0a] px-6 py-3 text-sm font-medium hover:bg-[#b8e600] transition disabled:opacity-50">
          {{ submitting ? 'Saving...' : isNew ? 'Create Release' : 'Save Changes' }}
        </button>
        <NuxtLink to="/admin/releases" class="border border-[#333] hover:border-[#666] px-6 py-3 text-sm font-medium transition">
          Cancel
        </NuxtLink>
        <button v-if="!isNew" type="button" :disabled="deleting" @click="handleDelete" class="border border-red-900 text-red-400 hover:bg-red-950 px-6 py-3 text-sm font-medium transition disabled:opacity-50">
          {{ deleting ? 'Deleting...' : 'Delete' }}
        </button>
      </div>
    </form>

    <p v-else class="text-[#888] text-sm">Release not found.</p>
  </div>
</template>

<script setup lang="ts">
import { useReleaseStore } from '~/stores/releases.store';
import { uploadFile } from '~/services/api';

definePageMeta({ layout: 'admin', middleware: 'auth' });

const route = useRoute();
const store = useReleaseStore();

const isNew = computed(() => route.params.id === 'new');
const existing = computed(() => store.releases.find(r => r.id === route.params.id));

const form = reactive({
  title: '',
  formats: '',
  description: '',
  coverImageUrl: '',
  spotifyUrl: '',
  appleMusicUrl: '',
  youtubeUrl: '',
  releaseDate: '',
});

const uploading = ref(false);
const submitting = ref(false);
const deleting = ref(false);
const error = ref('');

onMounted(async () => {
  if (!store.releases.length) await store.fetchReleases();
  if (existing.value) populateForm(existing.value);
});

watch(existing, (release) => {
  if (release && !isNew.value) populateForm(release);
});

const populateForm = (release: NonNullable<typeof existing.value>) => {
  form.title = release.title;
  form.formats = release.formats;
  form.description = release.description;
  form.coverImageUrl = release.coverImageUrl;
  form.spotifyUrl = release.spotifyUrl || '';
  form.appleMusicUrl = release.appleMusicUrl || '';
  form.youtubeUrl = release.youtubeUrl || '';
  form.releaseDate = release.releaseDate.slice(0, 10);
};

const onFileChange = async (event: Event, field: 'coverImageUrl') => {
  const input = event.target as HTMLInputElement;
  const file = input.files?.[0];
  if (!file) return;
  uploading.value = true;
  try {
    const result = await uploadFile(file);
    form[field] = result.fileUrl;
  } catch (err) {
    error.value = 'File upload failed.';
  } finally {
    uploading.value = false;
  }
};

const handleSubmit = async () => {
  error.value = '';
  submitting.value = true;
  try {
    const payload = {
      title: form.title,
      formats: form.formats,
      description: form.description,
      coverImageUrl: form.coverImageUrl,
      spotifyUrl: form.spotifyUrl || undefined,
      appleMusicUrl: form.appleMusicUrl || undefined,
      youtubeUrl: form.youtubeUrl || undefined,
      releaseDate: new Date(form.releaseDate).toISOString(),
    };
    if (isNew.value) {
      const created = await store.createRelease(payload);
      await navigateTo(`/admin/releases/${created.id}`);
    } else if (existing.value) {
      await store.updateRelease(existing.value.id, payload);
    }
  } catch {
    error.value = isNew.value ? 'Failed to create release.' : 'Failed to save release.';
  } finally {
    submitting.value = false;
  }
};

const handleDelete = async () => {
  if (!existing.value || !confirm(`Delete "${existing.value.title}"? This cannot be undone.`)) return;

  deleting.value = true;
  error.value = '';
  try {
    await store.deleteRelease(existing.value.id);
    await navigateTo('/admin/releases');
  } catch {
    error.value = 'Failed to delete release.';
  } finally {
    deleting.value = false;
  }
};
</script>

<style scoped>
.input {
  @apply w-full bg-[#0a0a0a] border border-[#222] px-4 py-3 text-sm text-white focus:outline-none focus:border-[#ccff00] transition;
}
</style>
