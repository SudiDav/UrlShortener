<template>
  <div id="app" class="min-h-screen bg-gray-100 py-6 flex flex-col justify-center sm:py-12">
    <div class="relative py-3 sm:max-w-xl sm:mx-auto">
      <div class="absolute inset-0 bg-gradient-to-r from-cyan-400 to-light-blue-500 shadow-lg transform -skew-y-6 sm:skew-y-0 sm:-rotate-6 sm:rounded-3xl"></div>
      <div class="relative px-6 py-12 bg-white shadow-lg sm:rounded-3xl sm:p-24">
        <h1 class="text-4xl font-bold mb-8 text-center text-gray-800">URL Shortener</h1>
        <div v-if="isAuthenticated">
          <UrlShortener class="mb-8" />
          <UrlList />
          <button @click="logout" class="mt-4 w-full py-2 px-4 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-red-600 hover:bg-red-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-red-500">
            Logout
          </button>
        </div>
        <div v-else>
          <Register v-if="showRegister" @toggle="toggleAuth" @register-success="handleRegisterSuccess" />
          <Login v-else @toggle="toggleAuth" @login-success="handleAuthSuccess" />
          <button @click="toggleAuth" class="mt-4 text-sm text-indigo-600 hover:text-indigo-500">
            {{ showRegister ? 'Already have an account? Login' : 'Need an account? Register' }}
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import UrlShortener from '@/components/UrlShortener.vue'
import UrlList from '@/components/UrlList.vue'
import Register from '@/components/Register.vue'
import Login from '@/components/Login.vue'

const isAuthenticated = ref(false)
const showRegister = ref(false)

const toggleAuth = () => {
  showRegister.value = !showRegister.value
}

const handleAuthSuccess = () => {
  isAuthenticated.value = true
}

const handleRegisterSuccess = () => {
  showRegister.value = false
}

const logout = () => {
  localStorage.removeItem('token')
  isAuthenticated.value = false
}

onMounted(() => {
  const token = localStorage.getItem('token')
  isAuthenticated.value = !!token
})
</script>