import React, { useState,useEffect } from 'react';
import { uploadImage,addArt,addBid } from './api/art';
import { useRouter } from 'next/router';
import Link from 'next/link'

// import axios from 'axios';

const CreateArt = () => {
  const [image, setImage] = useState(null);
  const [title, setTitle] = useState('');
  const [minBid, setMinBid] = useState(0);
  const [maxBid, setMaxBid] = useState(0);
  const [bidExpiry, setBidExpiry] = useState('');
  const [description, setDescription] = useState('');
  const router = useRouter();
  //useEffect to check localstorage.getItem('token'), if token doesnt exist redirect to login page

  useEffect(() => {
    console.log(localStorage.getItem('token'));
    if(!localStorage.getItem('token')|| localStorage.getItem('token') === "null"){
      console.log("token not found");
      router.push('/login');
    }
  },[]);


  const handleSubmit = async (e) => {
    e.preventDefault();
    const formData = new FormData();
    var obj = {
        title: title,
        MinBid: minBid,
        MaxBid: maxBid,
        BidExpiry: bidExpiry,
        description: description
    }

    try {
      const data = await uploadImage(image);
      if (data.fileName) {
        obj.artURL = `https://localhost:7005/api/Arts/getFile?fileName=${data.fileName}`;
        console.log(obj);
        const artData = await addArt(obj);
        // console.log('full',);
        if (artData) {
          
          const bidData = await addBid({...artData,...obj});
          if (bidData) {
            alert("Art Created Successfully");
            console.log(bidData);
            window.location.href = "/explore";
          }
        }
      }
    } catch (err) {
      console.log(err);
      alert("Error in creating Art");
    }
    
  };

  const handleImageUpload = (e) => {
    setImage(e.target.files[0]);
  };

  return (
    <div className="flex flex-col h-screen">
    <nav className="flex justify-between items-center bg-white py-4 px-8 shadow">
        <div className="flex items-center">
          <Link href="/explore">
            <div className="text-xl font-bold text-gray-800 hover:text-gray-600 mr-4">Explore</div>
          </Link>
          <Link href="/my-art">
            <div className="text-xl font-bold text-gray-800 hover:text-gray-600 mr-4">My Art</div>
          </Link>
        </div>
        <Link href="/createArt">
          <div className="bg-indigo-600 hover:bg-indigo-700 text-white py-2 px-4 rounded">
            Create Art
          </div>
        </Link>
      </nav>
    <div className="bg-gray-200 min-h-screen flex flex-col items-center justify-center">
            
      {image ? (
        <form onSubmit={handleSubmit} className="w-1/2 bg-white shadow-md rounded px-8 pt-6 pb-8 mb-4">
          <label className="block mb-4">
            Title
            <input type="text" value={title} onChange={(e) => setTitle(e.target.value)} className="w-full px-4 py-2 rounded border-2 border-gray-400" required />
          </label>
          <label className="block mb-4">
            Description
            <input type="description" value={description} onChange={(e) => setDescription(e.target.value)} className="w-full px-4 py-2 rounded border-2 border-gray-400" required />
          </label>
          <label className="block mb-4">
            Minimum Bid
            <input type="number" value={minBid} onChange={(e) => setMinBid(e.target.value)} className="w-full px-4 py-2 rounded border-2 border-gray-400" required />
          </label>
          <label className="block mb-4">
            Maximum Bid
            <input type="number" value={maxBid} onChange={(e) => setMaxBid(e.target.value)} className="w-full px-4 py-2 rounded border-2 border-gray-400" required />
          </label>
          <label className="block mb-4">
            Bid Expiry
            <input type="date" value={bidExpiry} onChange={(e) => setBidExpiry(e.target.value)} className="w-full px-4 py-2 rounded border-2 border-gray-400" required />
          </label>
          <button type="submit" className="px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600">Create Art</button>
        </form>
      ) : (
        <div className="bg-white border-2 border-dashed border-gray-400 rounded-lg p-8 mb-4 flex flex-col items-center justify-center">
          <p className="mb-4">Drag and drop your image here or click to upload</p>
          <input type="file" accept="image/*" name = "formFile" onChange={handleImageUpload} />
        </div>
      )}
    </div>
    </div>
  );
};

export default CreateArt;
