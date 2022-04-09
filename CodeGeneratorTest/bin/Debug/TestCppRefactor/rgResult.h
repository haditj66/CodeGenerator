#pragma once

namespace pref{
int r = 0;
//namespace blaablaa {
	template<class T>
	class rg
	{
	public:
		static int blaa;
		T bloop() const;

		rg(int t);
		~rg();
	};
	//}

template <class T>
T rg<T>::bloop() const
{
}

#define fef 3

	//comment
	//Annonymous
	//Attributes
	//

template<class T>
	int rg<T>::blaa = r;

	
}//pref