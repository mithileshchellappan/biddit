import { useState } from 'react';
import Image from 'next/image';
import { useForm } from 'react-hook-form';
import { useRouter } from 'next/router';
import { login, signUp } from './api/auth';
import { setToken } from '../utils/cookie';

const LoginPage = () => {
  const [isLogin, setIsLogin] = useState(true); 
  const { register, handleSubmit } = useForm();
  const router = useRouter();

  const onSubmit = async (formData) => {
    try {
      var result;
      if (isLogin) {
         result = await login(formData.username, formData.password); 
         
      } else {
         result = await signUp( formData.username,formData.password); 
      }
      if(result){
        console.log(result);
        router.push('/explore'); 
      }
    } catch (error) {
      console.error(error);
    }
  };

  return (
    <div className="flex items-center justify-center h-screen">
      <div className="flex flex-col md:flex-row w-full max-w-5xl p-8 md:p-16 bg-gray-100 rounded-lg shadow-lg">

        <div className="md:w-1/2 order-1 md:order-2">
          <h2 className="text-2xl font-bold mb-4">{isLogin ? 'Login' : 'Sign Up'}</h2>
          <form onSubmit={handleSubmit(onSubmit)}>
            <label htmlFor="username" className="block font-medium mb-2">
              Username
            </label>
            <input
              id="username"
              type="username"
              {...register('username', { required: true })}
              className="w-full border-gray-400 border-2 rounded-lg px-3 py-2 mb-4"
            />

            <label htmlFor="password" className="block font-medium mb-2">
              Password
            </label>
            <input
              id="password"
              type="password"
              {...register('password', { required: true })}
              className="w-full border-gray-400 border-2 rounded-lg px-3 py-2 mb-4"
            />
            <button
              type="submit"
              className="bg-indigo-600 text-white py-2 px-4 rounded-lg font-medium"
            >
              {isLogin ? 'Login' : 'Sign Up'}
            </button>
          </form>
          <p className="mt-4">
            {isLogin ? "Don't have an account yet?" : 'Already have an account?'}
            <button
              className="ml-2 underline font-medium text-indigo-600"
              onClick={() => setIsLogin(!isLogin)}
            >
              {isLogin ? 'Sign up' : 'Login'}
            </button>
          </p>
        </div>
      </div>
    </div>
  );
};

export default LoginPage;
