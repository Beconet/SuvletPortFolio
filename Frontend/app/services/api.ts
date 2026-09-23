export const fetchApi = async <T>(endpoint: string, options: Record<string, any> = {}): Promise<T> => {
  const config = useRuntimeConfig();
  const token = useCookie<string | null>('admin_token');
  try {
    return await $fetch<T>(`${config.public.apiBase}${endpoint}`, {
      ...options,
      headers: {
        ...(options.headers || {}),
        ...(token.value ? { Authorization: `Bearer ${token.value}` } : {}),
      },
    });
  } catch (err: any) {
    if (err?.response?.status === 401) {
      token.value = null;
    }
    throw err;
  }
};

export interface UploadFileResponse {
  fileUrl: string;
  folder: string;
  fileSizeBytes: number;
}

export const uploadFile = async (file: File): Promise<UploadFileResponse> => {
  const config = useRuntimeConfig();
  const token = useCookie<string | null>('admin_token');
  const formData = new FormData();
  formData.append('file', file);

  return await $fetch<UploadFileResponse>(`${config.public.apiBase}/api/storage/upload`, {
    method: 'POST',
    body: formData,
    headers: token.value ? { Authorization: `Bearer ${token.value}` } : {},
  });
};