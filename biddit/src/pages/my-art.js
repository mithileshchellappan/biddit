import { useState, useEffect } from 'react';
import Image from 'next/image';
import Link from 'next/link';
import { getArt } from './api/art';
import Router from 'next/router';
const MyArtPage = () => {
  const [art, setArt] = useState([]);
  const router = Router;

  useEffect(() => {
    const fetchArt = async () => {
      const token = localStorage.getItem('token')
      if (token||token!=="null") {
        const artData = await getArt(0,true); 
        console.log(artData);
        setArt(artData);
      }else{
        router.push('/login')
      }

    };
    fetchArt();
  }, []);

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
      <div className="flex-grow p-8">
        <h2 className="text-2xl font-bold mb-8">My Art</h2>
        <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-8">
          {art.map((artwork) => (
            <Link key={artwork.id} href={`/art/${artwork.artId}`}>
              <div className="block rounded-lg overflow-hidden shadow">
                <div className="relative">
                  <Image
                    src={artwork.artURL}
                    alt={artwork.title}
                    width={400}
                    height={400}
                    className="object-cover"
                    unoptimized
                  />
                  {/* {artwork.bids.length > 0 && (
                    <div className="absolute inset-0 bg-gray-900 bg-opacity-75 flex items-center justify-center">
                      <p className="text-white text-lg font-bold">
                        Highest bid: ${artwork.bids[0].amount}
                      </p>
                    </div>
                  )} */}
                </div>
                <div className="bg-white p-4">
                  <h3 className="text-lg font-bold">{artwork.title}</h3>
                  <p className="text-gray-600">by {artwork.userName}</p>
                </div>
              </div>
            </Link>
          ))}
        </div>
      </div>
    </div>
  );
};

export default MyArtPage;
