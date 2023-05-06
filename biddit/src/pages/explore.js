import { useState, useEffect } from 'react';
import Image from 'next/image';
import Link from 'next/link';
import { getArt } from './api/art';
import logoutUser  from './api/logoutUser';
import Router from 'next/router';
import styles from './ExplorePage.module.css';


const ExplorePage = () => {
  const [art, setArt] = useState([]);
  const router = Router;

  useEffect(() => {
    const fetchArt = async () => {
      const token = localStorage.getItem('token');
      if (token || token !== 'null') {
        const artData = await getArt();
        if(!artData){
          logoutUser()
          router.push('/login');
        }
        console.log(artData);
        setArt(artData);
      } else {
        router.push('/login');
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
        <h2 className="text-2xl font-bold mb-8">Explore Art</h2>
        <div className={styles.grid}>
          {art.map((artwork) => (
            <Link key={artwork.artId} href={`/art/${artwork.artId}`} onClick={()=>{router.push(`/art/${artwork/artId}`)}}>
              <div className={styles.tile}>
                <div className={styles.imageWrapper}>
                  <Image
                    src={artwork.artURL}
                    alt={artwork.title}
                    width={400}
                    height={400}
                    className={styles.image}
                    unoptimized
                  />
                </div>
                <div className={styles.tooltip}>
                  <h3 className={styles.title}>{artwork.title}</h3>
                  <p className={styles.userName}>by {artwork.userName}</p>
                </div>
              </div>
            </Link>
          ))}
        </div>
      </div>
    </div>
  );
};

export default ExplorePage;
